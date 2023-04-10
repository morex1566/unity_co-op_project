using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public interface ILevel
{
    public void OnPauseTimer();
    public void OnResumeTimer();
    public void OnStartTimerFor(float time);
    public float OnGetTime();

    public void AddGameObjectsToLevel(List<GameObject> objs);
    public void AddGameObjectToLevel(GameObject obj);
}

public interface ILevelPlatformSpawner : ILevel
{
    // INFO 사용되는 Prefab들...
    GameObject PlatformPrefab { get; }
    float MapSpeed { get; }
    float PlatformWidth { get; }
    float PlatformLength { get; }
    float PlatformHeight { get; }
    float LevelStartPos { get; }
    float PlatformLayerCount { get; }
}

public interface ILevelObstacleSpawner : ILevel
{
    GameObject FragileObstaclePrefab { get; }
    GameObject StaticObstaclePrefab { get; }
    float SyncSpeed { get; }
    float FragileObstacleSize { get; }
    float StaticObstacleSize { get; }
}

public class GameManager : MonoBehaviour, ILevelPlatformSpawner, ILevelObstacleSpawner
{
    public static GameManager Instance = null;
    
    [Header("Dependencies")]
    [Space(5)]
    [SerializeField] private GameObject levelPlatformSpawner;
    [SerializeField] private GameObject levelObstacleSpawner;
    [SerializeField] private GameObject levelTimer;
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
    [SerializeField] private float platformLayerCount;
        
    [Header("LevelObstacleSpawner Setting")]
    [Space(5)]
    [SerializeField] private GameObject fragileObstaclePrefab;
    [SerializeField] private GameObject staticObstaclePrefab;
    [SerializeField] private float fragileObstacleSize;
    [SerializeField] private float staticObstacleSize;
    
    [Header("Song Information")]
    [Space(5)]
    [HideInInspector][SerializeField] private AudioSource audioSource;
    [HideInInspector][SerializeField] private string name;
    
    // INFO : Script Cache
    private LevelTimer _levelTimer;
    private LevelPlatformSpawner _levelPlatformSpawner;
    private LevelObstacleSpawner _levelObstacleSpawner;

    private MapData _mapData;
    private int _healthCount = 10;

    // INFO : ILevelPlatformSpawner 구현
    public GameObject PlatformPrefab => platformPrefab;
    public float MapSpeed => mapSpeed;
    public float PlatformWidth => platformWidth;
    public float PlatformLength => platformLength;
    public float PlatformHeight => platformHeight;
    public float LevelStartPos => levelStartPos;
    public float PlatformLayerCount => platformLayerCount;
    
    // INFO : ILevelObstacleSpawner 구현
    public GameObject FragileObstaclePrefab => fragileObstaclePrefab;
    public GameObject StaticObstaclePrefab => staticObstaclePrefab;
    public float SyncSpeed => syncSpeed;
    public float FragileObstacleSize => fragileObstacleSize;
    public float StaticObstacleSize => staticObstacleSize;

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
        
        _levelTimer = levelTimer.GetComponent<LevelTimer>();
        _levelPlatformSpawner = levelPlatformSpawner.GetComponent<LevelPlatformSpawner>();
        _levelObstacleSpawner = levelObstacleSpawner.GetComponent<LevelObstacleSpawner>();
        audioSource = GetComponent<AudioSource>();

        level = Instantiate(level);
    }

    private void Start()
    {
        bool result;
        
        // TODO : 이부분 나중에 사용할 때 편집.
        result = LoadLevelData();
        if (!result)
        {
            Debug.Log("LoadMapData error");   
        }
        
        // ACTION : 시작하면 '[Header("Dependencies")]'에 있는 객체들에게 이벤트를 보냅니다.
        OnStartTimerFor(10f);
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
    private bool LoadLevelData()
    {
        _mapData = FileLoadManager.LoadMapData("Streaming-Heart.wav.txt");
        if (_mapData != null)
        {
            name = _mapData.Filename;
            audioSource.clip = _mapData.clip;

            return true;
        }

        return false;
    }

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
    public void OnStartTimerFor(float time)
    {
        StartCoroutine(_levelTimer.StartTimerFor(time));
    }

    // INFO : 타이머의 현재 시간을 얻습니다.
    public float OnGetTime()
    {
        return _levelTimer.GetTime();
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
}
