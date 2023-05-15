using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    [SerializeField]
    GameObject player;
    
    private void Start()
    {
        StartCoroutine(MakeObject());
    }

    IEnumerator MakeObject()
    {
        //randomXPositon = Random.Range(2, -2);
        GameObject box1 = Instantiate(box, new Vector3(player.transform.position.x+1, 1f, -15f), Quaternion.identity);
        box1.GetComponent<BoxCollider>().enabled = true;
        GameObject box2 = Instantiate(box, new Vector3(player.transform.position.x - 1, 1f, -15f), Quaternion.identity);
        box2.GetComponent<BoxCollider>().enabled = true;
        
        yield return null;
        //StartCoroutine(MakeObject());
    }
}
