using Server;
using System.Text.RegularExpressions;
using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageData : MonoBehaviour
{
    [SerializeField] private Image image;
    public MapData MapData;

    [SerializeField] private GameObject scoreBlock;
    [SerializeField] private GameObject scoreContents;
    
    AsyncOperationHandle<TextAsset> handle;

    private void Start()
    {
        // File의 이름을 등록
        string modifiedFilename = Regex.Replace(MapData.Filename, @"(\.txt|\.wav|\.mp3)", "");
        if (image)
        {
            image.GetComponentInChildren<TextMeshProUGUI>().text = modifiedFilename;
        }

        getScoreData();
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

    private void getScoreData()
    {
        ServerManager.Instance.LoadRank(Regex.Replace(MapData.Filename, @"(\.txt|\.wav|\.mp3)", ""), scoreContents, scoreBlock);
    }

    private void CheckIsAddressable(string filePath)
    {
        Debug.Log(filePath);

        Addressables.LoadAssetAsync<TextAsset>(Regex.Replace(MapData.Filename, @"(\.txt|\.wav|\.mp3)", "")).Completed 
        += (AsyncOperationHandle<TextAsset> obj) =>
        {
            handle = obj;
            if (obj.Result != null)
            {
                Debug.Log("File is Addressable.");
            }
            
            Addressables.Release(handle);
        };
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
