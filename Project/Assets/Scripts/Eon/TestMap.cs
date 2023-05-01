using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    GameObject health;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fragile Obstacle")
        {
            Destroy(health.transform.GetChild(health.transform.childCount-1).gameObject);
            Debug.Log("qqq");
        }
    }

}
