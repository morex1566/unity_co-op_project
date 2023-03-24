using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public float step_timer = 0.0f; // ��� �ð��� �����Ѵ�.
    private PlayerControl player = null;


    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        this.step_timer += Time.deltaTime; // ��� �ð��� ���� ����.

        if (this.player.isPlayEnd())
        {
            SceneManager.LoadScene("Title");
        }
    }

    public float getPlayTime()
    {
        float time;
        time = this.step_timer;
        return (time); // ȣ���� ���� ��� �ð��� �˷��ش�.
    }
}
