using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorAnimation : MonoBehaviour
{
    private MapEditorEventHandler eventHandler;

    void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
    }
}
