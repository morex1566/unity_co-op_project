/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 모든 정보를 가지고 있는 DB담당
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;


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

public struct SongData
{
    public string Filename;
    public string FilePath;
    public AudioSource source;
}

public class MapEditorManager : MonoBehaviour
{
    private MapEditorEventHandler eventHandler;
    private SongData song;

    void Awake()
    {
        song = new SongData();
        song.FilePath = null;
        song.Filename = null;
        song.source = GetComponent<AudioSource>();
        
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSong(string filepath, string filename, AudioClip audioClip)
    {
        song.FilePath = filepath;
        song.Filename = filename;
        song.source.clip = audioClip;
        song.source.clip.name = filename;
        song.source.Play();
    }
}
