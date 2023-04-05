/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  사용자가 하는 행동들(Event)을 각 MapEditor~ 스크립트에 전달해주는 라우터
 *               1. 노래 스크롤바
 *               2. 장애물 생성, 파일 입출력 버튼
 *               3. 맵UI
 */

using UnityEngine;

[ExecuteInEditMode]
public class MapEditorEventHandler : MonoBehaviour
{
    [SerializeField] private MapEditorManager editorManager;
    [SerializeField] private MapEditorUI editorUI;

    void Awake()
    {
        editorManager = GameObject.Find("EditorManager").GetComponent<MapEditorManager>();
        editorUI = GameObject.Find("UI").GetComponent<MapEditorUI>();
    }


    #region ButtonEvent

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

    }
    // ACTION : 현재 맵 플레이를 중단
    public void OnStopClicked()
    {

    }
    // ACTION : 장애물을 생성
    public void OnCreateClicked()
    {

    }
    // ACTION : 노래를 불러오기
    public void OnSongClicked()
    {
        editorManager.OpenSong();

    }

    #endregion
}
