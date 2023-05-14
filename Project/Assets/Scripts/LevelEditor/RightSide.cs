using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSide : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForDestory(0.5f));
    }

    public IEnumerator WaitForDestory(float time)
    {
        yield return new WaitForSeconds(time);
        
        Destroy(gameObject);
    }
}
