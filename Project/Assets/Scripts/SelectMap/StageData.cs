using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageData : MonoBehaviour
{
    [SerializeField] private Image image;
    public MapData MapData;

    private void Start()
    {
        // File의 이름을 등록
        string modifiedFilename = Regex.Replace(MapData.Filename, @"(\.txt|\.wav)", "");
        image.GetComponentInChildren<TextMeshProUGUI>().text = modifiedFilename;
    }

    public void OnPlayMap()
    {
        // DonDestroyOnLoad()로 MainScene에 전달할 정보를 살려둡니다.
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        
        // Scene 전환
        {
            // SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Editor");
        }
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     var manager = FindObjectOfType<GameManager>();
    //     manager.LoadLevelData(MapData);
    //     
    //     // 정보 전달을 마치면 파괴
    //     Destroy(gameObject);
    // }
}
