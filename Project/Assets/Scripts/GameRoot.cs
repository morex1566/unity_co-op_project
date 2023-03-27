using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public float step_timer = 0.0f;
    private PlayerControl player = null;


    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        this.step_timer += Time.deltaTime; 

        if (this.player.isPlayEnd())
        {
            SceneManager.LoadScene("Title");
        }
    }

    public float getPlayTime()
    {
        float time;
        time = this.step_timer;
        return (time); 
    }
}
