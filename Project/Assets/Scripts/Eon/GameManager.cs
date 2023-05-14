using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// TODO : 지금 GameManager는 모든 Scene에서 사용 가능할듯 합니다. 현재 Scene에서만 사용가능하도록 제한해야 할듯
public class GameManager : MonoBehaviour, IGameManagerPlatformSpawner, IGameManagerObstacleSpawner
{
    public static GameManager Instance = null;

    [Header("Dependencies")] 
    [Space(5)] 
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject resultBoard;

    [Header("LevelPlatformSpawner Setting")]
    [Space(5)]
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float mapSpeed;
    [SerializeField] private float syncSpeed;
    [SerializeField] private float platformWidth;
    [SerializeField] private float platformLength;
    [SerializeField] private float platformHeight;
    [SerializeField] private float levelStartPos;
    [SerializeField] private int platformLayerCount;
        
    [Header("LevelObstacleSpawner Setting")]
    [Space(5)]
    [SerializeField] private GameObject fragileObstaclePrefab;
    [SerializeField] private GameObject staticObstaclePrefab;
    [SerializeField] private GameObject spawnPointPrefab;
    [SerializeField] private float fragileObstacleSize;
    [SerializeField] private float staticObstacleSize;
    
    [Header("Song Information")]
    [Space(5)]
    private AudioSource _audioSource;
    [FormerlySerializedAs("name")] [HideInInspector][SerializeField] private string songName;
    
    // INFO : Script Cache
    private LevelTimer _levelTimer;
    public LevelPlatformSpawner _levelPlatformSpawner;
    public LevelObstacleSpawner _levelObstacleSpawner;

    private MapData _mapData;
    private int _healthCount = 10;
    public int StartDelay = 30;
    
    
    
    // INFO : IGameManager 구현
    // INFO : 타이머를 멈춥니다.
    public void OnPauseTimer()
    {
        _levelTimer.PauseTimer();
    }
    // INFO : 타이머를 재개합니다.
    public void OnResumeTimer()
    {
        _levelTimer.ResumeTimer();
    }
    // INFO : 타이머를 시작합니다. (시간 설정)
    public void OnStartTimerFor(TimePer unit, float time)
    {
        _levelTimer.StartTimerFor(unit, time);
    }
    // INFO : 타이머의 현재 시간을 얻습니다.
    public float OnGetTime(TimePer unit)
    {
        return _levelTimer.Current();
    }
    // ACTION : 추가한 GameObjects가 Level의 child가 됩니다.
    public void AddGameObjectsToLevel(List<GameObject> objs)
    {
        foreach (var obj in objs)
        {
            obj.transform.SetParent(level.transform);
        }
    }
    // ACTION : 추가한 GameObject가 Level의 child가 됩니다.
    public void AddGameObjectToLevel(GameObject obj)
    {
        obj.transform.SetParent(level.transform);
    }
    public GameObject GetLevel()
    {
        return level;
    }
    public Vector3 GetCenterPointAtLevel()
    {
        float x = 0;
        float y = (float)((platformWidth * 0.5f * 0.5f) * Math.Sqrt(3)) * 2;
        float z = levelStartPos + (platformLayerCount * platformLength * 0.5f);

        return new Vector3(x, y, z);
    }
    // ACTION : 플레이어와 장애물 스폰 포인트 사이의 거리
    public float GetDistance()
    {
        return Math.Abs(Math.Abs(levelStartPos) - Math.Abs(player.transform.position.z));
    }
    public MapData MapData => _mapData;
    public AudioSource AudioSource => _audioSource;

    public int HealthCount => _healthCount;
    // INFO : ILevelPlatformSpawner 구현
    public GameObject PlatformPrefab => platformPrefab;
    public float MapSpeed => mapSpeed;
    public float PlatformWidth => platformWidth;
    public float PlatformLength => platformLength;
    public float PlatformHeight => platformHeight;
    public float LevelStartPos => levelStartPos;
    public int PlatformLayerCount => platformLayerCount;
    
    // INFO : ILevelObstacleSpawner 구현
    public GameObject FragileObstaclePrefab => fragileObstaclePrefab;
    public GameObject StaticObstaclePrefab => staticObstaclePrefab;
    public GameObject SpawnPointPrefab => spawnPointPrefab;
    public float SyncSpeed => syncSpeed;
    public float FragileObstacleSize => fragileObstacleSize;
    public float StaticObstacleSize => staticObstacleSize;
    public float AtTime => _levelTimer.At();

    private void Awake()
    {
        // 싱글톤
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _audioSource = GetComponent<AudioSource>();
        
        bool result = LoadLevelData();
        if (!result)
        {
            Debug.Log("LoadMapData error");   
        }

        _levelTimer = new LevelTimer();
        _levelObstacleSpawner = new LevelObstacleSpawner();
        _levelObstacleSpawner.Initialize();
        _levelPlatformSpawner = new LevelPlatformSpawner();
        _levelPlatformSpawner.Initialize();
    }

    private void Start()
    {
        StartCoroutine(StartPlayAt(StartDelay));
        
        // ACTION : 시작하면 '[Header("Dependencies")]'에 있는 객체들에게 이벤트를 보냅니다.
        OnStartTimerFor(TimePer.Milisec, _mapData.timeline + StartDelay);
        _levelObstacleSpawner.Start();
    }

    private IEnumerator StartPlayAt(float delay)
    {
        yield return new WaitForSeconds(delay / 10);
        
        _audioSource.Play();
    }

    private void Update()
    {
        _levelPlatformSpawner.Update();
        HealthCheck();
    }

    public void HealthCheck()
    {
        if (health.transform.childCount == 0)
        {
            OnCreateResultBoard();
        }
    }

    // INFO : 여기서 우리가 만든 맵핑 데이터와 음악을 불러옵니다.
    // public bool LoadLevelData(MapData mapData)
    // {
    //     _mapData = mapData;
    //     if (_mapData != null)
    //     {
    //         songName = _mapData.Filename;
    //         audioSource.clip = _mapData.clip;
    //
    //         return true;
    //     }
    //
    //     return false;
    // }

    // INFO : 여기서 우리가 만든 맵핑 데이터와 음악을 불러옵니다.
    public bool LoadLevelData()
    {
        var stageData = FindObjectOfType<StageData>();
        _mapData = stageData.MapData;
        if (_mapData != null)
        {
            songName = _mapData.Filename;
            _audioSource.clip = _mapData.clip;
        
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        _levelObstacleSpawner.CancelStart();
    }

    /// <summary>
    /// 음악 소리를 Fade-Out하며 종료합니다
    /// </summary>
    /// <param name="duration">진행되는 시간</param>
    /// <param name="targetVolume">목표 음량</param>
    public IEnumerator OnMusicFadeOut(float duration, float targetVolume = 0f)
    {
        while (_audioSource.volume > targetVolume)
        {
            _audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }
    }
    
    /// <summary>
    /// 음악 소리를 Fade-In하며 시작합니다
    /// </summary>
    /// <param name="duration">진행되는 시간</param>
    /// <param name="targetVolume">목표 음량</param>
    public IEnumerator OnMusicFadeIn(float duration, float targetVolume = 1.0f)
    {
        // TODO : maxVolume부분 하드코딩 수정 필요
        while (_audioSource.volume < targetVolume)
        {
            _audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }
    }

    /// <summary>
    /// ResultBoard를 활성화 시키는 이벤트 함수
    /// </summary>
    public void OnCreateResultBoard()
    {
        // 내부 함수
        IEnumerator createResultBoard()
        {
            // 장애물이 완전히 도착할 때까지 대기
            yield return new WaitForSeconds(GetDistance() / mapSpeed * 1.5f);
        
            resultBoard.SetActive(true);
        
            // 소리 음량 감소
            StartCoroutine(OnMusicFadeOut(0.5f, 0.2f));
        }
        
        StartCoroutine(createResultBoard());
    }


    /// <returns> Fragile Obstacle의 갯수 </returns>
    public int GetMaxCombo()
    {
        return _levelObstacleSpawner.FragileObstacleCount;
    }


    /// <returns> 내가 Cut한 Fragile Obstacle의 갯수 </returns>
    /// TODO : 이부분 Sword에서 가져와서 수정하는거?
    public int GetCombo()
    {
        return 0;
    }

    public int GetHealthCount()
    {
        return health.transform.childCount;
    }
}
