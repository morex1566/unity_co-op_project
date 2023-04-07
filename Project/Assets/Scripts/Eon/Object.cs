using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;

    GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed);
        if(player.transform.position.z+10 <gameObject.transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    
}
