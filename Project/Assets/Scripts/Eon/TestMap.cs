using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TestMap : MonoBehaviour
{

    [SerializeField]
    GameObject health;

    [SerializeField]
    GameObject stopImg;

    [FormerlySerializedAs("audio")] [SerializeField]
    AudioSource _audio;

    public AudioMixer audioMixer;

    public Slider BgmSlider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fragile Obstacle")
        {
            Destroy(health.transform.GetChild(health.transform.childCount-1).gameObject);
            Debug.Log("qqq");
        }
        
        if (collision.gameObject.tag == "Static Obstacle")
        {
            Destroy(health.transform.GetChild(health.transform.childCount-1).gameObject);
            Debug.Log("qqq");
        }
    }
    bool isPause = true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Editor");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
