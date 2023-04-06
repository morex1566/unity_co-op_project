using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class SpriteConverter : MonoBehaviour {
 
    public Image image;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image> ();
    }
}