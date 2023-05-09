using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPool<T> where T : new()
{
    private Queue<T> _pool = new Queue<T>();
    public T Get()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }
        else
        {
            return new T();
        }
    }
    public void Return(T obj)
    {
        _pool.Enqueue(obj);
    }
}

/// <summary>
/// Level Scene의 장애물을 생성하는 클래스
/// </summary>
/// <remarks> 사용 전, Initialize()함수 호출 필수</remarks>
public class LevelObstacleSpawner
{
    private IGameManagerObstacleSpawner _gameManager;
    
    // 맵 정보
    private MapData _mapData;
    
    // Spawner관련 변수들 모음
    private GameObject _fragileObstaclePrefab;
    private GameObject _staticObstaclePrefab;
    private float _syncSpeed;
    private float _mapSpeed;
    private float _platformWidth;
    private float _fragileObstacleSize;
    private float _staticObstacleSize;
    private float _levelStartPos;

    // 맵과 장애물 사이의 간격
    private float _spawnOffset;

    // 오브젝트 풀링용 객체
    private ObjectPool<GameObject> _fragileObstaclePool;
    private ObjectPool<GameObject> _staticObstaclePool;
    
    // 객체 생성 Timeline queue
    // Pair<스폰 시간, List<스폰 위치>>
    private Queue<KeyValuePair<int, int>> _fragileSpawnTimeline;
    // 객체 생성 Timeline queue
    // Pair<스폰 시간, List<Pair<기둥 지나는 점1, 기둥 지나는 점2>>>
    private Queue<KeyValuePair<int, KeyValuePair<int, int>>> _staticObstacleTimeline;
    private Queue<KeyValuePair<int, int>> _holeObstacleTimline;

    private List<GameObject> _currReleasedObstacles;
    private List<Vector3> _spawnPoints;
    
    // 초기화되었는지 확인합니다. Start()를 호출할 때, 필요합니다.
    private bool _isInit = false;
    private Task _spawner;
    private CancellationTokenSource _spawnerCancellation;

    public int FragileObstacleCount;


    // INFO : GameManager의 MapData를 기반으로, 장애물 Timeline, spawn point를 맵핑합니다.
    public void Initialize()
    {
        // GameManager 클래스의 ILevelObstacleSpawner 인터페이스 참조
        _gameManager = (GameManager.Instance) as IGameManagerObstacleSpawner;

        _mapData = _gameManager.MapData;
        
        // 게임 매니저에서 정보 가져오기
        _fragileObstaclePrefab = _gameManager.FragileObstaclePrefab;
        _staticObstaclePrefab = _gameManager.StaticObstaclePrefab;
        _syncSpeed = _gameManager.SyncSpeed;
        _fragileObstacleSize = _gameManager.FragileObstacleSize;
        _staticObstacleSize = _gameManager.StaticObstacleSize;
        _platformWidth = _gameManager.PlatformWidth;
        _levelStartPos = _gameManager.LevelStartPos;
        
        // TODO : 이 부분 하드코딩 config
        _spawnOffset = 3f;

        _mapSpeed = _gameManager.MapSpeed;
        
        _currReleasedObstacles = new List<GameObject>();
        _spawnPoints = new List<Vector3>();

        _fragileSpawnTimeline = new Queue<KeyValuePair<int, int>>();
        _staticObstacleTimeline = new Queue<KeyValuePair<int, KeyValuePair<int, int>>>();
        _holeObstacleTimline = new Queue<KeyValuePair<int, int>>();

        
        // _spawnPoints의 위치를 할당
        getSpawnPosition();
        CreateSpawnTimeline(0);

        FragileObstacleCount = _fragileSpawnTimeline.Count;

        _isInit = true;
    }
    public void Start()
    {
        if (_isInit)
        {
            _spawnerCancellation = new CancellationTokenSource();
            _spawner = createObstacle(_spawnerCancellation.Token);
        }
        else
        {
            Debug.Log(GetType().Name + " : [NOTICE] Start()를 사용하기 전에, Initialize()를 먼저 호출.");
        }
    }
    private async Task createObstacle(CancellationToken token)
    {
        while ( _fragileSpawnTimeline.Count > 0 || _staticObstacleTimeline.Count > 0 || _holeObstacleTimline.Count > 0)
        {
            int sync = (int)((_gameManager.GetDistance() / _mapSpeed) * 10);
            float time = _gameManager.OnGetTime(TimePer.Milisec);
            
            // 종료되었는지 확인
            if (token.IsCancellationRequested) { return; }

            // 부서지는 오브젝트 생성
            while (_fragileSpawnTimeline.Count > 0 && _fragileSpawnTimeline.Peek().Key - (sync) <= time)
            {
                
                // 장애물의 회전목표 계산
                Vector3 start = _spawnPoints[_fragileSpawnTimeline.Peek().Value];
                Vector3 dest = _gameManager.GetCenterPointAtLevel();
                        dest.z = _spawnPoints[0].z; 
                Vector3 dir = dest - start;
                
                // 장애물을 인스턴싱합니다
                GameObject obstacle = UnityEngine.Object.Instantiate(
                    _fragileObstaclePrefab,
                    _spawnPoints[_fragileSpawnTimeline.Peek().Value],
                    Quaternion.LookRotation(dir));
                
                // 관리될 수 있도록 컨테이너에 삽입
                _currReleasedObstacles.Add(obstacle);
                _gameManager.AddGameObjectToLevel(obstacle);
    
                // 생성했으니, 대기열에서 제거
                _fragileSpawnTimeline.Dequeue();
            }
    
            // 정적 오브젝트 생성
            while (_staticObstacleTimeline.Count > 0 && _staticObstacleTimeline.Peek().Key - (sync) <= time)
            {
                // 정적 오브젝트의 회전값 계산
                Vector3 start = _spawnPoints[_staticObstacleTimeline.Peek().Value.Key];
                Vector3 dest = _spawnPoints[_staticObstacleTimeline.Peek().Value.Value];
                Quaternion rot = Quaternion.LookRotation(start - dest);
                
                // _spawnPoints들의 중앙
                Vector3 spawnPoint = _gameManager.GetCenterPointAtLevel();
                        spawnPoint.z = _spawnPoints[0].z;
                        
                // 장애물을 인스턴싱 합니다
                GameObject obstacle = UnityEngine.Object.Instantiate(_staticObstaclePrefab, spawnPoint, rot);
                
                // 관리될 수 있도록 컨테이너에 삽입
                _currReleasedObstacles.Add(obstacle);
                _gameManager.AddGameObjectToLevel(obstacle);
    
                // 생성했으니, 대기열에서 제거
                _staticObstacleTimeline.Dequeue();
            }
            
            // 구멍 오브젝트 생성
            while (_holeObstacleTimline.Count > 0 && _holeObstacleTimline.Peek().Key - (sync) <= time)
            {
                // 쏴야 할 Ray의 방향 
                Vector3 start = _gameManager.GetCenterPointAtLevel();
                        start.z = _spawnPoints[0].z + 2;
                Vector3 dest = _spawnPoints[_holeObstacleTimline.Peek().Value];
                        dest.z += 2;
                Vector3 dir = dest - start;

                
                // Ray를 쏴서 맞춘 오브젝트를 비활성화 시킵니다.
                RaycastHit hit;
                if (Physics.Raycast(start, dir, out hit, 40f))
                {
                    hit.transform.gameObject.SetActive(false);
                }
                
                for (int i = 0; i < 5; i++)
                {
                    _holeObstacleTimline.Dequeue();
                }
            }
            
            await Task.Yield();
        }
        
        // 모든 obstacle 생성 완료. result board 생성 이벤트 호출
        done();
    }
    public void CancelStart()
    {
        _spawnerCancellation.Cancel();
    }
    private void CreateSpawnTimeline(int start)
    {
        for (int i = 0; i < _mapData.timeline; i++)
        {
            bool staticObstacleFound = false;
            
            for (int j = 0; j < MapEditorUI.timelineYPixelCount; j++)
            {
                Color color = _mapData.map.GetPixel(i, j);

                if (color.Equals(Color.red))
                {
                    _fragileSpawnTimeline.Enqueue(new KeyValuePair<int, int>(i, j));
                }
                else 
                if (color.Equals(Color.blue))
                {
                    _holeObstacleTimline.Enqueue(new KeyValuePair<int, int>(i, j));
                }
                else 
                if (color.Equals(Color.green) && !staticObstacleFound)
                {
                    int point1 = j;
                    int point2 = 0;
                    for (int k = j + 1; k < MapEditorUI.timelineYPixelCount; k++)
                    {
                        if (color.Equals(Color.green)) { point2 = k; }
                    }
                    
                    _staticObstacleTimeline.Enqueue(new KeyValuePair<int, KeyValuePair<int, int>>(i,
                                                        new KeyValuePair<int, int>(point1, point2)));

                    staticObstacleFound = true;
                }
            }
        }
    }
    // CAUTION : 내부 for문 배치순서를 바꾸면 안됩니다. 바꾼다면 MapEditor의 장애물 놓는 곳도 변경
    private void getSpawnPosition()
    {
        int sliceCount = 6; // 하드 코딩된 값

        float horizontalWidthOffset = _platformWidth / sliceCount;
        float diagonalWidthOffset = _platformWidth * 0.5f / sliceCount;

        float heightOffset = (float)(((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3)) * 2) / sliceCount;
        float platformHeight = (float)((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3));

        Vector3 centerPoint = _gameManager.GetCenterPointAtLevel();
        
        // 하단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) - (_platformWidth * 0.5f) + (horizontalWidthOffset * i),
                (centerPoint.y) - (platformHeight * 2),
                _levelStartPos
            );
            
            // 장애물과 platform 사이의 offset 계산
            spawnPoint = spawnPoint - (Vector3.down * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }
        
        // 좌측 하단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) + (_platformWidth * 0.5f) + (diagonalWidthOffset * i),
                (centerPoint.y) - (platformHeight * 2) + (heightOffset * i),
                _levelStartPos
            );
            
            // 장애물과 platform 사이의 offset 계산
            Vector3 dir = Vector3.down;  
            float angle = 60f;  
            Vector3 axis = new Vector3(0, 0, 1);  

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);  
            Vector3 rotatedUp = rotation * dir;  

            spawnPoint = spawnPoint - (rotatedUp * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }
        
        // 좌측 상단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) + (_platformWidth) - (diagonalWidthOffset * i),
                (centerPoint.y) + (heightOffset * i),
                _levelStartPos
            );
            
            // 장애물과 platform 사이의 offset 계산
            Vector3 dir = Vector3.up;  
            float angle = -60f;  
            Vector3 axis = new Vector3(0, 0, 1);  

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);  
            Vector3 rotatedUp = rotation * dir;  

            spawnPoint = spawnPoint - (rotatedUp * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }
        
        // 상단
        for (int i = 1; i < 6; i++)
        {
            // point 지점 계산
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) - (_platformWidth * 0.5f) + (horizontalWidthOffset * i),
                (centerPoint.y) + (platformHeight * 2),
                _levelStartPos
            );

            // 장애물과 platform 사이의 offset 계산
            spawnPoint = spawnPoint - (Vector3.up * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }

        // 우측 상단
        for (int i = 1; i < 6; i++)
        {
            // point 지점 계산
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) - (_platformWidth * 0.5f) - (diagonalWidthOffset * i),
                (centerPoint.y) + (platformHeight * 2) - (heightOffset * i),
                _levelStartPos
            );
            
            // 장애물과 platform 사이의 offset 계산
            Vector3 dir = Vector3.up;  
            float angle = 60f;  
            Vector3 axis = new Vector3(0, 0, 1);  

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);  
            Vector3 rotatedUp = rotation * dir;  

            spawnPoint = spawnPoint - (rotatedUp * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }
        
        // 우측 하단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) - (_platformWidth) + (diagonalWidthOffset * i),
                (centerPoint.y) - (heightOffset * i),
                _levelStartPos
            );
            
            // 장애물과 platform 사이의 offset 계산
            Vector3 dir = Vector3.down;  
            float angle = -60f;  
            Vector3 axis = new Vector3(0, 0, 1);  

            Quaternion rotation = Quaternion.AngleAxis(angle, axis);  
            Vector3 rotatedUp = rotation * dir;  

            spawnPoint = spawnPoint - (rotatedUp * _spawnOffset);
            
            _spawnPoints.Add(spawnPoint);
        }
    }

    /// <summary>
    /// CALL BY : 매핑된 장애물이 전부 소환한 뒤에 호출
    /// </summary>
    private void done()
    {
        // GameManager에게 종료 UI요청
        GameManager.Instance.OnCreateResultBoard();
    }
}
