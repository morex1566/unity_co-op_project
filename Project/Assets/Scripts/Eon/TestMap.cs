using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="Fragile Obstacle")
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            Material material = renderer.materials[0];
            MeshCut.Cut(collision.gameObject, collision.transform.position, Vector3.right, material);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("LeftAttack");
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("RightAttack");
        }
    }
}
