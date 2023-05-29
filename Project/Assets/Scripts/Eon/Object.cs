using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;

    GameObject player;

    public bool boxStop = false;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void Update()
    {
        if(player.transform.position.z-1 > transform.position.z)
            transform.Translate(Vector3.forward * speed);       
        else
        {
            boxStop = true;
        }
        if(player.transform.position.z+10 <gameObject.transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
