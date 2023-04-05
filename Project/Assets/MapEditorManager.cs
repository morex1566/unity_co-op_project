/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 모든 정보를 가지고 있는 DB담당
 */

using UnityEngine;
using UnityEngine.Networking;
using System.IO;


public class EditorHotKey
{
    // ACTION : 작업 세이브
    public static EventModifiers SaveCurrentWorkModifiers = EventModifiers.Control;
    public static KeyCode SaveCurrentWorkKey = KeyCode.S;

    // ACTION : 이전 작업으로
    public static EventModifiers UndoCurrentWorkModifiers = EventModifiers.Control;
    public static KeyCode UndoCurrentWork = KeyCode.Z;

    // ACTION : 앞 작업으로
    public static EventModifiers RedoCurrentWorkModifiers = EventModifiers.Control | EventModifiers.Shift;
    public static KeyCode RedoCurrentWork = KeyCode.Z;

    // ACTION : 플레이 시작
    public static EventModifiers PlayCurrentWorkModifiers = EventModifiers.Control;
    public static KeyCode PlayCurrentWork = KeyCode.F5;

    // ACTION : 플레이 멈추기
    public static EventModifiers StopCurrentWorkModifiers = EventModifiers.Control | EventModifiers.Shift;
    public static KeyCode StopCurrentWork = KeyCode.F5;
}


struct SongData
{
    public string    Filename;
    public string    route;
    public AudioClip source;
}


public class MapEditorManager : MonoBehaviour
{
    private SongData song;
    private MapEditorAnimation uiAnimation;
    private MapEditorFilestream fileIO;

    void Awake()
    {
        uiAnimation = new MapEditorAnimation();
        fileIO = new MapEditorFilestream();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ACTION : 노래를 불러오는게 성공했다면 TRUE
    public bool OpenSong()
    {
        return true;
    }
}
