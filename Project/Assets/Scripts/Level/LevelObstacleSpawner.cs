using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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

public class LevelObstacleSpawner : MonoBehaviour
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

    private void Start()
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
        _spawnOffset = 1f;

        _mapSpeed = _gameManager.MapSpeed;
        
        _currReleasedObstacles = new List<GameObject>();
        _spawnPoints = new List<Vector3>();

        _fragileSpawnTimeline = new Queue<KeyValuePair<int, int>>();
        _staticObstacleTimeline = new Queue<KeyValuePair<int, KeyValuePair<int, int>>>();
        _holeObstacleTimline = new Queue<KeyValuePair<int, int>>();

        
        // _spawnPoints의 위치를 할당
        getSpawnPosition();
        setSpawnTimeline(0);
    }

    private void Update()
    {
        createObstacle((float)_gameManager.OnGetTime(TimePer.Milisec));
    }

    // TODO : 여기에 static이랑 hole도 추가해주세요 싱크 맞춰야함
    private void createObstacle(float time)
    {
        double sync = _gameManager.GetDistance() / _mapSpeed;

  
        while (_fragileSpawnTimeline.Count > 0 && _fragileSpawnTimeline.Peek().Key - (sync) <= time)
        {
            _currReleasedObstacles.Add(Instantiate(
                _fragileObstaclePrefab,
                _spawnPoints[_fragileSpawnTimeline.Peek().Value],
                quaternion.identity));

            _fragileSpawnTimeline.Dequeue();
        }

        // if (_staticObstacleTimeline.Peek().Key - (sync) <= time)
        // {
        //     
        // }
        //
        // if (_holeObstacleTimline.Peek().Key - (sync) <= time)
        // {
        //     
        // }
    }

    private void setSpawnTimeline(int start)
    {
        for (int i = 0; i < _mapData.timeline; i++)
        {
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
                if (color.Equals(Color.green))
                {
                    int point1 = j;
                    int point2 = 0;
                    for (int k = j + 1; k < MapEditorUI.timelineYPixelCount; k++)
                    {
                        if (color.Equals(Color.green)) { point2 = k; }
                    }
                    
                    _staticObstacleTimeline.Enqueue(new KeyValuePair<int, KeyValuePair<int, int>>(i,
                                                        new KeyValuePair<int, int>(point1, point2)));
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
    }
}
