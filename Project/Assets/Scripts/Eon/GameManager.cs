using System;
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
    [SerializeField] private float fragileObstacleSize;
    [SerializeField] private float staticObstacleSize;
    
    [Header("Song Information")]
    [Space(5)]
    [HideInInspector][SerializeField] private AudioSource audioSource;
    [FormerlySerializedAs("name")] [HideInInspector][SerializeField] private string songName;
    
    // INFO : Script Cache
    private LevelTimer _levelTimer;
    private LevelPlatformSpawner _levelPlatformSpawner;
    private LevelObstacleSpawner _levelObstacleSpawner;

    private MapData _mapData;
    private int _healthCount = 10;
    
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
        
        audioSource = GetComponent<AudioSource>();
        
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
        audioSource.Play();

        // ACTION : 시작하면 '[Header("Dependencies")]'에 있는 객체들에게 이벤트를 보냅니다.
        OnStartTimerFor(TimePer.Milisec, _mapData.timeline);
        _levelObstacleSpawner.Start();
    }

    private void Update()
    {
        _levelPlatformSpawner.Update();
    }

    public void HealthCheck()
    {
        if (_healthCount == 1)
        {
            SceneManager.LoadScene("Editor");
        }
        health.transform.GetChild(_healthCount - 1).gameObject.SetActive(false);
        _healthCount--;
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
            audioSource.clip = _mapData.clip;
        
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        _levelObstacleSpawner.CancelStart();
    }
}
