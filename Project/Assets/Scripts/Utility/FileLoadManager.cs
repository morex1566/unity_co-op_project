using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;

public struct MapData
{
    public string Filename;
    public string Filepath;
    public AudioClip clip;
    public Texture2D map;
}

public static class FileLoadManager
{
    private static int timeout;
    
    // INFO : MapEditor에서 생성한 txt를 StreamingAssets 폴더에서 찾습니다. (filename 확장자포함)
    public static MapData LoadMapData(string filename)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, filename);
        string extension = Path.GetExtension(fullPath);
        MapData mapData = new MapData();

        string[] datasets;

        timeout = 10;
        
        
        if (extension != ".txt")
        {
            Debug.Log("it is not txt!");
            throw new Exception("Invalid file extension");
        }
        

        using (StreamReader reader = new StreamReader(fullPath))
        {
            var readTask = reader.ReadToEnd();
            
            string fileContents = readTask;
            datasets = fileContents.Split("\r\n");
        }

        mapData.Filepath = fullPath.Replace(".txt", "");
        mapData.Filename = filename;
    

        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + fullPath.Replace(".txt", ""), AudioType.WAV);
        www.SendWebRequest();

        while (!www.isDone)
        {
            
        }

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error loading audio clip: " + www.error);
        }
        else
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            
            // Audio Clip을 만들어 AudioSource에 연결
            mapData.clip = audioClip;
        }

        mapData.map = new Texture2D(datasets.Length - 4, MapEditorUI.timelineYPixelCount);
        // 초기화
        for (int i = 0; i < datasets.Length - 4; i++)
        {
            for (int j = 0; j < MapEditorUI.timelineYPixelCount; j++)
            {
                // CAUTION : 색깔 바꾸지 말아주세요! 바꾼다면 OnPositionMarkerButtonClicked(int index)
                // 함수에서도 if-check하는 color값도 동일하게 바꿔주세요!
                // TODO : 이 색깔부분 자유롭게 바꿀 수 있도록 변수화 필요!
                Color color = new Color(0, 0, 0, 0.9f);
                mapData.map.SetPixel(i, j, color);
            }
        }
        
        for (int i = 3; i < datasets.Length - 4; i++)
        {
            string[] command = datasets[i].Split("/");
            int time = 0;
 
            for (int j = 0; j < command.Length; j++)
            {
                int pos = 0;
                switch (command[j][0])
                {
                    case 'T':
                        time = Int32.Parse(command[j].Substring(1));
                        break;
                    
                    case 'F':
                        pos = Int32.Parse(command[j].Substring(1));
                        mapData.map.SetPixel(time, pos, Color.red);
                        
                        break;
                    
                    case 'H':
                        pos = Int32.Parse(command[j].Substring(1));
                        mapData.map.SetPixel(time, pos, Color.blue);
 
                        break;
                    
                    case 'S':
                        string[] data = command[j].Substring(1).Split("&");
                        int from = Int32.Parse(data[0]);
                        int to = Int32.Parse(data[1]);
                        
                        mapData.map.SetPixel(time, from, Color.green);
                        mapData.map.SetPixel(time, to, Color.green);
 
                        break;
                }
            }
        }
        
        mapData.map.Apply();

        return mapData;
    }
    
}