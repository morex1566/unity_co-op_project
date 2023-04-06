/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 전반적인 UI디자인 담당
 *               1. 노래 스크롤바
 *               2. 장애물 생성, 파일 입출력 버튼
 *               3. 맵UI
 */

using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;



[RequireComponent(typeof(LineRenderer))]
public class MapEditorUI :  MonoBehaviour
{
    private int blockPositionCount = 30;
    
    // 여기서 Inspector에 보이는 색깔에 따른 블럭 정보가 나타납니다.
    public enum ObstacleType 
    {
        FragileObstacle,
        StaticObstacle,
        Hole
    }


    // INFO : 각 MapEditor클래스끼리 통신을 위한 객체
    private MapEditorEventHandler eventHandler;

    // INFO : 맵UI 관련
    private LineRenderer lineRenderer;
    private Material LineColorMat;
    private float sideLength;

    // INFO : 메뉴UI 관련
    [SerializeField] private GameObject menuLayoutField;
    [SerializeField] private Button saveFileButton;
    [SerializeField] private Button loadFileButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button createObstacleButton;
    [SerializeField] private Button songButton;
    [SerializeField] private GameObject obstacleMenuGUI;
    [SerializeField] private GameObject obstaclePositionMarker;
    private ObstacleType currObstacleType;
    

    // INFO : 노래관련 
    [SerializeField] private Slider timelineSlider;
    private SliderAccessor sliderAccessor;
    [SerializeField] private GameObject songName;
    private Texture2D timelineInspector;

    public GameObject SongName { get { return songName; } set { songName = value; } }

    void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
        lineRenderer = GetComponent<LineRenderer>();
        LineColorMat = new Material(Shader.Find("Unlit/Texture"));
        sideLength = 7.5f;

        currObstacleType = ObstacleType.FragileObstacle;
    }
    
    void Start()
    {
        renderMap();
        renderTimelineInspector();

        saveFileButton.onClick.AddListener(eventHandler.OnSaveClicked);
        loadFileButton.onClick.AddListener(eventHandler.OnLoadClicked);
        playButton.onClick.AddListener(eventHandler.OnPlayClicked);
        stopButton.onClick.AddListener(eventHandler.OnStopClicked);
        createObstacleButton.onClick.AddListener(eventHandler.OnCreateClicked);

        songButton.onClick.AddListener(eventHandler.OnSongClicked);
    }

    private void renderTimelineInspector()
    {
        int width;
        width = (int)(timelineSlider.transform.GetComponent<RectTransform>() as RectTransform).rect.width;
        
        Debug.Log(width);
        
        // TODO : height값 조정할 수도 있습니다.
        timelineInspector = new Texture2D(width, blockPositionCount);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < blockPositionCount; j++)
            {
                Color color = new Color(0, 0, 0, 150);
                timelineInspector.SetPixel(i, j, color);
            }
        }

        timelineInspector.filterMode = FilterMode.Point;
        timelineInspector.Apply();
        Sprite inspectorSprite = Sprite.Create(timelineInspector,
            new Rect(0, 0, timelineInspector.width, timelineInspector.height), new Vector2(0.5f, 0.5f));

        (timelineSlider.transform.GetChild(0).GetComponent<Image>() as Image).sprite = inspectorSprite;
    }
    
    private void renderMap()
    {
        Vector3 center = transform.position;
        Color white = Color.white;

        lineRenderer.material = LineColorMat;

        Vector3[] positions = new Vector3[7];
        for (int i = 0; i < 6; i++)
        {
            float angle = i * Mathf.PI / 3f;
            positions[i] = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * sideLength;
        }

        positions[6] = positions[0];

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void ShowObstacleTypeMenuGUI()
    {
        obstacleMenuGUI.SetActive(true);
        
        StartCoroutine(showObstacleTypeMenuDialog());
    }

    private IEnumerator showObstacleTypeMenuDialog()
    {
        while (true)
        {
            
        }

        ShowObstaclePositionMark();
    }
    
    // INNER FUNCTION : 
    private void ShowObstaclePositionMark()
    {
        
    }

    public void SetTimeline(AudioClip audioClip)
    {
        int width = (int)Math.Round(audioClip.length * 10);
        timelineSlider.maxValue = width;
    }
    
    public float GetSongCurrentTime()
    {
        return timelineSlider.value / 10f;
    }

    public void SetSongCurrentTime(float time)
    {
        int modifiedTime = (int)Math.Round(time * 10);
        timelineSlider.value = modifiedTime;
    }
}
