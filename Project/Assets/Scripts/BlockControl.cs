using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public MapCreator map_creator = null; 
    void Start()
    {
       
        map_creator = GameObject.Find("GameManager").GetComponent<MapCreator>();
    }
    void Update()
    {
        if (this.map_creator.isDelete(this.gameObject))
        { 
            GameObject.Destroy(this.gameObject); 
        }
    }
}
