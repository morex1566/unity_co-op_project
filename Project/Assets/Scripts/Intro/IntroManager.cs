using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public void Start()
    {
    }

    public void OnBackgroundMusicPlay()
    {
        BackgroundProcess.Instance.OnPlayMusic();
    }

    public void OnLoadTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
