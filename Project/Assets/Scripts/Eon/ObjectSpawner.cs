using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    float randomXPositon =0;

    private void Start()
    {
        StartCoroutine(MakeObject());
    }

    IEnumerator MakeObject()
    {
        randomXPositon = Random.Range(-1, -9);
        Instantiate(box, new Vector3(randomXPositon, 19.15f, -24.265f), Quaternion.identity);
        yield return new WaitForSeconds(2f);
        StartCoroutine(MakeObject());
    }
}
