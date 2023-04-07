using System.IO;
using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    public void CreateSaveFile(Slider timelineSlider, SongData songData)
    {
        // JSON 파일에 저장할 값을 담을 리스트
        string path = Application.streamingAssetsPath + "/" + songData.Filename.ToString() + ".json";
        StreamWriter writer = new StreamWriter(path, false);
        
        Texture2D timelineTexture = timelineSlider.
                                    GetComponent<RectTransform>().
                                    Find("Background").
                                    GetComponent<Image>().
                                    sprite.texture;
        
        // 첫줄에는 파일의 정보를 저장합니다.
        writer.WriteLine(songData.FilePath);
        // 둘째줄에는 곡의 이름을 저장합니다.
        writer.WriteLine(songData.Filename);
        
        writer.WriteLine();
        
        // 넷째줄부터 시작
        // 각 픽셀의 색상을 검사하여 리스트에 저장
        // 예 ) C/T23/F0.../X -> 생성 이벤트/시간은 23/Fragile타입 장애물(F) 위치는 0번버튼+.... / 종료
        for (int x = 0; x < timelineTexture.width; x++)
        {
            writer.Write("C");
            writer.Write("/");
            writer.Write("T");
            writer.Write(x.ToString());
            writer.Write("/");

            for (int y = 0; y < timelineTexture.height; y++)
            {
                Color pixelColor = timelineTexture.GetPixel(x, y);

                if (pixelColor.Equals(Color.red))
                {
                    writer.Write("F");
                    writer.Write(y.ToString());
                    writer.Write("/");
                }
            }
            
            writer.Write("X");
            writer.WriteLine();
        }
        
        writer.Close();
    }
}