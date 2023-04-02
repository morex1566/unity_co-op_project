using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject    _obstacle;
    [SerializeField] private float         _yPadding;

    // Start is called before the first frame update
    void Awake()
    {
        _yPadding = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // TEST : NOTE 생성
        if (Input.GetKeyDown(KeyCode.G))
        {
            createObstacle();
        }
    }

    // TEST : NOTE를 생성합니다.
    private void createObstacle()
    {
        if (_obstacle)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y += _yPadding;

            Instantiate(_obstacle, spawnPos, Quaternion.identity);
        }
    }
}
