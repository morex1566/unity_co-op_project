using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelPlatformSpawner : MonoBehaviour
{
    // GameManager 클래스의 ILevelPlatformSpawner 인터페이스
    private ILevelPlatformSpawner _gameManager;
    
    private GameObject _platformPrefab;
    private float _platformWidth;  // x
    private float _platformLength; // z
    private float _platformHeight; // y
    
    private float _mapSpeed;

    private int _platformLayerCount;
    private float _levelStartPos;
    private float _levelEndPos;
    
    private Queue<GameObject> _currReleasedPlatforms;

    private void Start()
    {
        _gameManager = GameManager.Instance as ILevelPlatformSpawner;
        
        // 게임 매니저에서 정보 가져오기
        _platformPrefab = _gameManager.PlatformPrefab;
        _platformWidth = _gameManager.PlatformWidth;
        _platformLength = _gameManager.PlatformLength;
        _platformHeight = _gameManager.PlatformHeight;
        _platformLayerCount = _gameManager.PlatformLayerCount;
        
        _mapSpeed = _gameManager.MapSpeed;

        _levelStartPos = _gameManager.LevelStartPos;
        _levelEndPos = _levelStartPos + (_platformLength * (_platformLayerCount - 1));
        Debug.Log(_levelEndPos);

        _currReleasedPlatforms = new Queue<GameObject>();

        createDummyHexagonLayerAt(_levelStartPos);

        for (int i = _platformLayerCount - 1; i >= 0; i--)
        {
            createHexagonLayerAt(_levelStartPos + (_platformLength * i));
        }
    }

    private void Update()
    {
        movePlatforms();
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
            _platformLength
        );
        
        // 하단
        GameObject bottom = Instantiate(_platformPrefab);
        bottom.transform.localScale = normalizedScale;
        bottom.transform.position = new Vector3(0, 0, z);
        
        objs.Add(bottom);
        _currReleasedPlatforms.Enqueue(bottom);
        // 우측하단
        GameObject rightBottom = Instantiate(_platformPrefab);
        rightBottom.transform.localScale = normalizedScale;
        rightBottom.transform.position = new Vector3(-width, height, z);
        rightBottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60));

        objs.Add(rightBottom);
        _currReleasedPlatforms.Enqueue(rightBottom);

        // 우측상단
        GameObject rightTop = Instantiate(_platformPrefab);
        rightTop.transform.localScale = normalizedScale;
        rightTop.transform.position = new Vector3(-width, height * 3, z);
        rightTop.transform.rotation = Quaternion.Euler(new Vector3(0,0,-120));
        
        objs.Add(rightTop);
        _currReleasedPlatforms.Enqueue(rightTop);

        // 상단
        GameObject top = Instantiate(_platformPrefab);
        top.transform.localScale = normalizedScale;
        top.transform.position = new Vector3(0, height * 4, z);
        top.transform.rotation = quaternion.Euler(new Vector3(0, 0, 0));
        
        objs.Add(top);
        _currReleasedPlatforms.Enqueue(top);

        // 좌측상단
        GameObject leftTop = Instantiate(_platformPrefab);
        leftTop.transform.localScale = normalizedScale;
        leftTop.transform.position = new Vector3(width, height * 3, z);
        leftTop.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 120));
        
        objs.Add(leftTop);
        _currReleasedPlatforms.Enqueue(leftTop);

        // 좌측하단
        GameObject leftBottom = Instantiate(_platformPrefab);
        leftBottom.transform.localScale = normalizedScale;
        leftBottom.transform.position = new Vector3(width, height, z);
        leftBottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 60));
        
        objs.Add(leftBottom);
        _currReleasedPlatforms.Enqueue(leftBottom);

        // level의 child로 올립니다.
        _gameManager.AddGameObjectsToLevel(objs);
    }
    private void createDummyHexagonLayerAt(float z)
    {
        List<GameObject> objs = new List<GameObject>();

        // dynamic programming 값
        float width = (_platformWidth * 0.5f) + (_platformWidth * 0.5f * 0.5f);
        float height = (float)((_platformWidth * 0.5f * 0.5f) * Math.Sqrt(3));
        
        // normalized scale 값
        Vector3 normalizedScale = new Vector3(
            _platformWidth,
            _platformHeight,
            _platformLength * 2
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
        
        _gameManager.AddGameObjectsToLevel(objs);
    }
    
    // ACTION : 플랫폼을 움직입니다. 시야에서 벗어나면 다시 앞으로 재배치
    private void movePlatforms()
    {
        float lastPos = getCurrLastHexagonLayer();
        Vector3 nextPos;

        List<GameObject> platforms = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            platforms.Add(_currReleasedPlatforms.Dequeue());
        }
        
        if (platforms[0].transform.position.z >= _levelEndPos + _platformLength)
        {
            foreach (var platform in platforms)
            {
                nextPos = platform.transform.position;
                nextPos.z = lastPos;

                platform.transform.position = nextPos;
            }
        }
        
        for (int i = 0; i < 6; i++)
        {
            _currReleasedPlatforms.Enqueue(platforms[i]);
        }
        
        foreach (var platform in _currReleasedPlatforms)
        {
            nextPos = new Vector3(
                platform.transform.position.x,
                platform.transform.position.y,
                platform.transform.position.z + (_mapSpeed * Time.deltaTime)
            );
                    
            platform.transform.position = nextPos;
        }
    }

    private float getCurrLastHexagonLayer()
    {
        GameObject[] myArray = _currReleasedPlatforms.ToArray();
        float pos = myArray[myArray.Length - 1].transform.position.z - _platformLength;

        return pos;
    }
}