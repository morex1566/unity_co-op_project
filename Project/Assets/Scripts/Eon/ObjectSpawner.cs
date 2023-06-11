using System.Collections;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    [SerializeField]
    GameObject player;

    [SerializeField]
    TutorialManager manager;


    private void Start()
    {
        StartCoroutine(MakeObject());
    }

    IEnumerator MakeObject()
    {
        GameObject box1 = Instantiate(box, new Vector3(player.transform.position.x+1, 1f, -15f), Quaternion.identity);
        box1.GetComponent<BoxCollider>().enabled = true;
        GameObject arrow = box1.transform.GetChild(0).gameObject;
        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x * -1, arrow.transform.localScale.y, arrow.transform.localScale.z);
        yield return new WaitUntil(()=> box1.GetComponent<Object>().boxStop && manager.left);
        GameObject box2 = Instantiate(box, new Vector3(player.transform.position.x - 1, 1f, -15f), Quaternion.identity);
        box2.GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }
}
