using System.IO;
using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapEditorFileIO : MonoBehaviour
{
    // INFO : 각 MapEditor클래스끼리 통신을 위한 객체
    private MapEditorEventHandler eventHandler;
    void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
    }
    public void OnFileBrowserOpen()
    {
    	FileBrowser.SetFilters(true, new FileBrowser.Filter("Audio Files", ".wav"));
    	FileBrowser.SetDefaultFilter(".wav");
    	FileBrowser.AddQuickLink("Users", "C:\\Users");
    	// Coroutine example
    	StartCoroutine(ShowLoadDialogCoroutine());
    }
    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");
        if (FileBrowser.Success)
        {
            string filename = FileBrowserHelpers.GetFilename(FileBrowser.Result[0]);
            string filepath = FileBrowser.Result[0];
            string copyPath = Path.Combine(Application.streamingAssetsPath, filename);

            FileBrowserHelpers.CopyFile(filepath, copyPath);

            yield return WaitForCopyFile(copyPath);
            yield return LoadAudioClipFromPath(copyPath, filename);
    	}
    }
    
    // INNER METHOD TO ShowLoadDialogCoroutine() : 읽은 audio파일을 audioClip으로 변환합니다.
    // path = "StreamingAssets폴더 audio파일의 절대 경로"
    IEnumerator LoadAudioClipFromPath(string filepath, string filename)
    {
        // UnityWebRequest를 사용하여 파일을 가져옴
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filepath, AudioType.WAV);

        // 응답이 도착할 때까지 기다림
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading audio clip: " + www.error);
        }
        else
        {
            // Audio Clip을 만들어 AudioSource에 연결
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

            // Event Handler를 통해 EditorManager에게 전달
            eventHandler.SendSongDesc(filepath, filename, audioClip);
        }
    }
    
    // INNER METHOD TO ShowLoadDialogCoroutine() : 파일이 성공적으로 복사되었는지 확인
    // path = "StreamingAssets폴더 audio파일의 절대 경로"
    private IEnumerator WaitForCopyFile(string copyPath)
    {
        bool isCopied = false;
        float waitTime = 0.1f;  // 0.1초마다 파일 존재 여부를 검사합니다.
        float counter = 0f; // 타임오버 측정
        
        while (!isCopied)
        {
            yield return new WaitForSeconds(waitTime);
            counter += waitTime;

            // 파일이 존재하는지 확인
            if (File.Exists(copyPath))
            {
                isCopied = true;
                Debug.Log("File copy is completed: " + copyPath);
            }
            else
            {
                // 시간초과 
                if (counter > 10f)
                {
                    isCopied = true;
                    Debug.Log("load time out: "+ copyPath);
                }
            }
        }

        yield return isCopied;
    }
}