/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  각 MapEditor~ 스크립트들이 통신하는 라우터
 *  주의 사항 :  Event 함수 등록할 때, function chaining하면 stack-tracing하기 힘들어 집니다....
 */

using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class MapEditorEventHandler : MonoBehaviour
{
    // INFO : 등록된 통신망 이용자들
    [SerializeField] private MapEditorManager editorManager;
    [SerializeField] private MapEditorUI editorUI;
    [SerializeField] private MapEditorAnimation editorAnimation;
    [SerializeField] private MapEditorFileIO editorFileIO;

    void Awake()
    {
        editorManager = GameObject.Find("Editor").GetComponent<MapEditorManager>();
        editorUI = GameObject.Find("UI").GetComponent<MapEditorUI>();
        editorAnimation = GameObject.Find("Editor").GetComponent<MapEditorAnimation>();
        editorFileIO = GameObject.Find("Editor").GetComponent<MapEditorFileIO>();
    }

    #region BUTTON_EVENT
    [SerializeField]
    // ACTION : 현재까지 만든 맵 저장
    public void OnSaveClicked()
    {

    }
    // ACTION : 만든 맵을 불러오기
    public void OnLoadClicked()
    {
        
    }
    // ACTION : 현재까지 만든 맵을 플레이
    public void OnPlayClicked()
    {
        editorManager.PlaySongAt(editorUI.GetSongCurrentTime());
    }
    // ACTION : 현재 맵 플레이를 중단
    public void OnStopClicked()
    {
        editorManager.StopSongAt();
    }
    // ACTION : 장애물을 생성
    public void OnCreateClicked()
    {
        editorUI.ShowObstacleTypeMenuGUI();
    }
    // ACTION : 노래를 불러오기
    public void OnSongClicked()
    {
        editorFileIO.OnFileBrowserOpen();
    }

    #endregion

    #region USER_EVENT
    
    // ACTION : OnSongClicked()에 대한 회신, 읽은 파일 정보 전달
    public void SendSongDesc(string filepath, string filename, AudioClip audioClip)
    {
        editorManager.SetSong(filepath, filename, audioClip);
        editorUI.SongName.GetComponent<TextMeshProUGUI>().text = filename;
        editorUI.SetTimeline(audioClip);
    }

    public void SetSongCurrentTime(float songCurrentTime)
    {
        editorUI.SetSongCurrentTime(songCurrentTime);
    }
    
    #endregion
}
