using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    [SerializeField]
    GameObject player;

    float randomXPositon =0;

    private void Start()
    {
        StartCoroutine(MakeObject());
    }

    IEnumerator MakeObject()
    {
        //randomXPositon = Random.Range(2, -2);
        Instantiate(box, new Vector3(player.transform.position.x+1, 1f, -15f), Quaternion.identity);
        Instantiate(box, new Vector3(player.transform.position.x - 1, 1f, -15f), Quaternion.identity);
        yield return null;
        //StartCoroutine(MakeObject());
    }
}
