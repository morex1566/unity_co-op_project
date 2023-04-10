using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacleSpawner : MonoBehaviour
{
    private GameObject _fragileObstaclePrefab;
    private GameObject _staticObstaclePrefab;
    private float _syncSpeed;
    private float _fragileObstacleSize;
    private float _staticObstacleSize;

    private List<GameObject> _currReleasedObjects;
    
    private void Start()
    {
        // GameManager 클래스의 ILevelObstacleSpawner 인터페이스 참조
        ILevelObstacleSpawner gameManager = (GameManager.Instance) as ILevelObstacleSpawner;
        
        // 게임 매니저에서 정보 가져오기
        _fragileObstaclePrefab = gameManager.FragileObstaclePrefab;
        _staticObstaclePrefab = gameManager.StaticObstaclePrefab;
        _syncSpeed = gameManager.SyncSpeed;
        _fragileObstacleSize = gameManager.FragileObstacleSize;
        _staticObstacleSize = gameManager.StaticObstacleSize;
    }

    private void Update()
    {
        
    }

    private void createObstacle()
    {
        // TODO: 장애물 생성 코드
    }
    
    private void getSpawnPosition()
    {
        
    }
}
