using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    // INFO : �ӵ� ��, ������ �������� ���� ��ü
    private LevelManager _levelManager;

    private Rigidbody _rigid;
    private Vector3 nextPos;

    void Awake()
    {
        _levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
        _rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigid.MovePosition(nextPos);
    }

    // Update is called once per frame
    void Update()
    {
        nextPos = transform.position;
        nextPos.z -= _levelManager.GameSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (transform.position.z < -5)
        {
            Destroy(this);
        }
    }
}
