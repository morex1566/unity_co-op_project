using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacleSpawner : MonoBehaviour
{
    private ILevelObstacleSpawner gameManager;
    
    private GameObject _fragileObstaclePrefab;
    private GameObject _staticObstaclePrefab;
    private float _syncSpeed;
    private float _platformWidth;
    private float _fragileObstacleSize;
    private float _staticObstacleSize;
    private float _levelStartPos;
    
    // 맵과 장애물 사이의 간격
    private float _spawnOffset;

    private List<GameObject> _currReleasedObstacles;
    private List<Vector3> _spawnPoints;

    private void Start()
    {
        // GameManager 클래스의 ILevelObstacleSpawner 인터페이스 참조
        gameManager = (GameManager.Instance) as ILevelObstacleSpawner;
        
        // 게임 매니저에서 정보 가져오기
        _fragileObstaclePrefab = gameManager.FragileObstaclePrefab;
        _staticObstaclePrefab = gameManager.StaticObstaclePrefab;
        _syncSpeed = gameManager.SyncSpeed;
        _fragileObstacleSize = gameManager.FragileObstacleSize;
        _staticObstacleSize = gameManager.StaticObstacleSize;
        _platformWidth = gameManager.PlatformWidth;
        _levelStartPos = gameManager.LevelStartPos;
        
        // TODO : 이 부분 하드코딩 config
        _spawnOffset = 1f;

        _currReleasedObstacles = new List<GameObject>();
        _spawnPoints = new List<Vector3>();
        
        // _spawnPoints의 위치를 할당
        getSpawnPosition();

        // test
        foreach (var point in _spawnPoints)
        {
            GameObject cubeInstance = Instantiate(_fragileObstaclePrefab, point, Quaternion.identity);
        }
    }

    private void Update()
    {
        
    }

    private void createObstacle()
    {
        // TODO: 장애물 생성 코드
    }
    
    
    // CAUTION : 내부 for문 배치순서를 바꾸면 안됩니다. 바꾼다면 MapEditor의 장애물 놓는 곳도 변경
    private void getSpawnPosition()
    {
        int sliceCount = 6; // 하드 코딩된 값

        float horizontalWidthOffset = _platformWidth / sliceCount;
        float diagonalWidthOffset = _platformWidth * 0.5f / sliceCount;

        float heightOffset = (float)(((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3)) * 2) / sliceCount;
        float platformHeight = (float)((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3));

        Vector3 centerPoint = gameManager.GetCenterPointAtLevel();
        
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
                (centerPoint.x) + (_platformWidth * 0.5f) + (diagonalWidthOffset * i),
                (centerPoint.y) + (platformHeight * 2) - (heightOffset * i),
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
        
        // 우측 하단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) + (_platformWidth) - (diagonalWidthOffset * i),
                (centerPoint.y) - (heightOffset * i),
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
        
        // 하단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) + (_platformWidth * 0.5f) - (horizontalWidthOffset * i),
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
                (centerPoint.x) - (_platformWidth * 0.5f) - (diagonalWidthOffset * i),
                (centerPoint.y) - (platformHeight * 2) + (heightOffset * i),
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
        
        // 좌측 상단
        for (int i = 1; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3(
                (centerPoint.x) - (_platformWidth) + (diagonalWidthOffset * i),
                (centerPoint.y) + (heightOffset * i),
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
    }
}
