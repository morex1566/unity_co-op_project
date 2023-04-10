using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelPlatformSpawner : MonoBehaviour
{
    // GameManager 클래스의 ILevelPlatformSpawner 인터페이스
    ILevelPlatformSpawner _gameManager;
    
    private GameObject _platformPrefab;
    private float _platformWidth;  // x
    private float _platformLength; // z
    private float _platformHeight; // y
    
    private float _mapSpeed;

    private float _platformLayerCount;
    private float _levelStartPos;
    private float _levelEndPos;
    private void Start()
    {
        _gameManager = GameManager.Instance as ILevelPlatformSpawner;
        
        // 게임 매니저에서 정보 가져오기
        _platformPrefab = _gameManager.PlatformPrefab;
        _mapSpeed = _gameManager.MapSpeed;
        _platformWidth = _gameManager.PlatformWidth;
        _platformLength = _gameManager.PlatformLength;
        _platformHeight = _gameManager.PlatformHeight;
        _platformLayerCount = _gameManager.PlatformLayerCount;
        _levelStartPos = _gameManager.LevelStartPos;
        
        for (int i = 0; i < _platformLayerCount; i++)
        {
            createHexagonLayerAt(_levelStartPos + (_platformLength * i));
        }

        _levelEndPos = _levelStartPos + (_platformLength * (_platformLayerCount - 1));
    }


    // ACTION : 육각형 platform 한 겹을 z값에 만듭니다.
    private void createHexagonLayerAt(float z)
    {
        List<GameObject> objs = new List<GameObject>();

        // dynamic programming 값
        float width = (_platformWidth * 0.5f) + (_platformWidth * 0.5f * 0.5f);
        float height = (float)((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3));
        
        // normalized scale 값
        Vector3 normalizedScale = new Vector3(
            _platformWidth,
            _platformHeight,
            -_platformLength
        );
        
        // 하단
        GameObject bottom = Instantiate(_platformPrefab);
        bottom.transform.localScale = normalizedScale;
        bottom.transform.position = new Vector3(0, 0, z);
        
        objs.Add(bottom);

        // 우측하단
        GameObject rightBottom = Instantiate(_platformPrefab);
        rightBottom.transform.localScale = normalizedScale;
        rightBottom.transform.position = new Vector3(-width, height, z);
        rightBottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60));

        objs.Add(rightBottom);
        
        // 우측상단
        GameObject rightTop = Instantiate(_platformPrefab);
        rightTop.transform.localScale = normalizedScale;
        rightTop.transform.position = new Vector3(-width, height * 3, z);
        rightTop.transform.rotation = Quaternion.Euler(new Vector3(0,0,-120));
        
        objs.Add(rightTop);

        // 상단
        GameObject top = Instantiate(_platformPrefab);
        top.transform.localScale = normalizedScale;
        top.transform.position = new Vector3(0, height * 4, z);
        top.transform.rotation = quaternion.Euler(new Vector3(0, 0, 0));
        
        objs.Add(top);

        // 좌측상단
        GameObject leftTop = Instantiate(_platformPrefab);
        leftTop.transform.localScale = normalizedScale;
        leftTop.transform.position = new Vector3(width, height * 3, z);
        leftTop.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 120));
        
        objs.Add(leftTop);

        // 좌측하단
        GameObject leftBottom = Instantiate(_platformPrefab);
        leftBottom.transform.localScale = normalizedScale;
        leftBottom.transform.position = new Vector3(width, height, z);
        leftBottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 60));
        
        objs.Add(leftBottom);
        
        // level의 child로 올립니다.
        _gameManager.AddGameObjectsToLevel(objs);
    }

    private void getSpawnPosition()
    {
        
    }
}