using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    float moveSpeed=0.5f;


    public Transform cuttingPlane;
    public LayerMask cuttableLayer;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        Vector3 Position = transform.position;
        Position.x += -1 * Horizontal * moveSpeed * Time.deltaTime;

        transform.position = Position;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Block")
    //    {

    //        Debug.Log("hi");
    //        //GameObject[] gameObjects = MeshCut.Cut(other.gameObject, transform.position, Vector3.down, other.gameObject.GetComponent<MeshRenderer>().material);
    //        GameManager.Instance.HealthCheck();
    //    }
    //}

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
