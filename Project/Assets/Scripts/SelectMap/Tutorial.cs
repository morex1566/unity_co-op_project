using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    /// <summary>
    /// 튜토리얼 Scene으로 Load됩니다
    /// </summary>
    public void OnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
