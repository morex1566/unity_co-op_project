using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utility;

public class TestMap : MonoBehaviour
{

    [SerializeField]
    Health health;
    
    [SerializeField]
    GameObject stopImg;

    [FormerlySerializedAs("audio")] [SerializeField]
    AudioSource _audio;

    [SerializeField] private AudioSource hitSound;

    [SerializeField] private Collider swordCollider;

    public AudioMixer audioMixer;

    public Slider BgmSlider;

    [SerializeField] private GameObject sword;
    private Sword _sword;

    private void Awake()
    {
        _sword = sword.GetComponent<Sword>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Static Obstacle")
        {
            health.SubHp();
            //Destroy(health.transform.GetChild(health.transform.childCount-1).gameObject);
            hitSound.Play();
        }
        
        
    }
    bool isPause = true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ResultBoard.Instance)
            {
                if (isPause)
                {
                    pause();
                }
                else
                {
                    Resume();
                }
            }
        }

    
    }

    void pause()
    {
        Time.timeScale = 0;
        stopImg.SetActive(true);
        _audio.Pause();
        isPause = false;
    }

    
    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        stopImg.SetActive(false);
        _audio.Play();
        isPause = true;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
    }
    
    private void OnAttackTrue()
    {
        swordCollider.enabled = true;
    }

    private void OnAttackFalse()
    {
        _sword.hitFlag = true;
        swordCollider.enabled = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
