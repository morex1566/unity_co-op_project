using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBlock : MonoBehaviour
{

    public float speed;
    public float scaleSpeed;


    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = transform.localPosition ;

        if (nextPos.z <= 0.5f)
        {
            nextPos.z += speed * Time.deltaTime;

            transform.localPosition = nextPos;
        }

        if(transform.localScale.x>1)
        {
            transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed)*Time.deltaTime;
        }
    }
}
