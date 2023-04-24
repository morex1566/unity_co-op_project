using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{ 
    public GameObject[] blockPrefabs; 
    private int block_count = 0; 

    public void createBlock(Vector3 block_position)
    {
        int next_block_type = this.block_count % this.blockPrefabs.Length; 
                                                                          
        GameObject go = GameObject.Instantiate(this.blockPrefabs[next_block_type]) as GameObject;
        go.transform.position = block_position; 
        this.block_count++; 
    }
}
