using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // INFO : ��ֹ��� �ӵ� � ����, ���� �ӵ�.
    [SerializeField] public float GameSpeed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameSpeed == 0f)
        {
            GameSpeed = 50f;
        }
    }
}