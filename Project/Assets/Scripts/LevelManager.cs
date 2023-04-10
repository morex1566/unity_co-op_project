using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public float GameSpeed = 0f;
    private MapData mapData;
    

    // Start is called before the first frame update
    void Awake()
    {
        if(GameSpeed == 0f)
        {
            GameSpeed = 50f;
        }

        mapData = FileLoadManager.LoadMapData("Streaming-Heart.wav.txt");

        if (mapData.clip == null)
        {
            Debug.Log("error");
        }
    }
}