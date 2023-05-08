using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectMapManager : MonoBehaviour
{
    private void Start()
    {
        BackgroundProcess.Instance.OnStopMusic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
}