using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 position_offset = Vector3.zero;
    void Start()
    {
        // ��� ���� player�� Player ������Ʈ�� �Ҵ�.
        this.player = GameObject.FindGameObjectWithTag("Player");
        // ī�޶� ��ġ(this.transform.position)�� �÷��̾� ��ġ(this.player.transform.position)�� ����.
        this.position_offset = this.transform.position - this.player.transform.position;
    }
    void LateUpdate()
    { // ��� ���� ������Ʈ�� Update() �޼��� ó�� �Ŀ� �ڵ����� ȣ��.
      // ī�޶� ���� ��ġ�� new_position�� �Ҵ�.
        Vector3 new_position = this.transform.position;
        // �÷��̾��� X��ǥ�� ���� ���� ���ؼ� new_position�� X�� ����.
        new_position.x = this.player.transform.position.x + this.position_offset.x;
        // ī�޶� ��ġ�� ���ο� ��ġ(new_position)�� ����.
        this.transform.position = new_position;
    }
}
