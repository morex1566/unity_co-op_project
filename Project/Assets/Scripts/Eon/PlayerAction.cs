using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    float moveSpeed=0.5f;

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        Vector3 Position = transform.position;
        Position.x += -1*Horizontal * moveSpeed * Time.deltaTime;

        transform.position = Position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            Debug.Log("hi");
            GameObject[] gameObjects = MeshCut.Cut(other.gameObject, transform.position, Vector3.down, other.gameObject.GetComponent<MeshRenderer>().material);
           // GameManager.instance.HealthCheck();
        }
    }
}
