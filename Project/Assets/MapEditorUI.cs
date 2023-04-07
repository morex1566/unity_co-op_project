/*
 *  업데이트  :  2023-04-07
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 전반적인 UI디자인 담당
 *               1. 노래 스크롤바
 *               2. 장애물 생성, 파일 입출력 버튼
 *               3. 맵UI
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(LineRenderer))]
public class MapEditorUI :  MonoBehaviour
{
    private int timelineYPixelCount = 30;

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
    [SerializeField] private Button createButton;
    [SerializeField] private Button songButton;
    [SerializeField] private GameObject obstacleMenuGUI;
    [SerializeField] private GameObject obstaclePositionMarker;
    private List<Button> positionMarkerButton;
    private ObstacleType currObstacleType;
    private bool createButtonToggle;
    

    // INFO : 노래관련 
    [SerializeField] private Slider timelineSlider;
    private SliderAccessor sliderAccessor;
    [SerializeField] private GameObject songName;

    public GameObject SongName { get { return songName; } set { songName = value; } }
    public bool CreateButtonToggle { get { return createButtonToggle; } set { createButtonToggle = value; } }
    
    
    void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
        sliderAccessor = timelineSlider.GetComponent<SliderAccessor>();
        lineRenderer = GetComponent<LineRenderer>();
        LineColorMat = new Material(Shader.Find("Unlit/Texture"));
        sideLength = 7.5f;

        currObstacleType = ObstacleType.FragileObstacle;
        createButtonToggle = false;
        obstacleMenuGUI.SetActive(false);
    }
    
    void Start()
    {
        renderMap();
        renderTimelineInspector();

        saveFileButton.onClick.AddListener(eventHandler.OnSaveClicked);
        loadFileButton.onClick.AddListener(eventHandler.OnLoadClicked);
        playButton.onClick.AddListener(eventHandler.OnPlayClicked);
        stopButton.onClick.AddListener(eventHandler.OnStopClicked);
        createButton.onClick.AddListener(eventHandler.OnCreateClicked);
        songButton.onClick.AddListener(eventHandler.OnSongClicked);

        // INFO : 장애물을 설치하는 지점에 버튼 이벤트를 등록합니다.
        positionMarkerButton = new List<Button>();
        positionMarkerButton.AddRange(obstaclePositionMarker.GetComponentsInChildren<Button>());
        for (int i = 0; i < positionMarkerButton.Count; i++)
        {
            int index = i; // 콜백 함수에서 사용하기 위해 인덱스 값을 저장합니다.
            positionMarkerButton[i].onClick.AddListener(() => OnPositionMarkerButtonClicked(index));
        }
    }

    // INFO : 하단 Timeline UI 드로우 콜
    private void renderTimelineInspector()
    {
        int width;
        width = (int)(timelineSlider.transform.GetComponent<RectTransform>() as RectTransform).rect.width;
        
        Texture2D timelineInspector;

        // TODO : height값 조정할 수도 있습니다.
        timelineInspector = new Texture2D(width, timelineYPixelCount);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < timelineYPixelCount; j++)
            {
                // CAUTION : 색깔 바꾸지 말아주세요! 바꾼다면 OnPositionMarkerButtonClicked(int index)
                // 함수에서도 if-check하는 color값도 동일하게 바꿔주세요!
                // TODO : 이 색깔부분 자유롭게 바꿀 수 있도록 변수화 필요!
                Color color = new Color(0, 0, 0, 0.9f);
                timelineInspector.SetPixel(i, j, color);
            }
        }

        timelineInspector.filterMode = FilterMode.Point;
        timelineInspector.Apply();
        Sprite inspectorSprite = Sprite.Create(timelineInspector,
            new Rect(0, 0, timelineInspector.width, timelineInspector.height), new Vector2(0.5f, 0.5f));
        
        (timelineSlider.transform.GetChild(0).GetComponent<Image>() as Image).sprite = inspectorSprite;
    }
    // INFO : 6각형 드로우 콜
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
    
    // INFO1 : 맵핑한 정보대로 버튼 색깔 변경
    // INFO2 : timeline을 조작할 때마다 이 함수가 호출
    public void UpdateTimelineInspector()
    {
        Texture2D  timelineInspectorTexture = timelineSlider.
                                              GetComponent<RectTransform>().
                                              Find("Background").
                                              GetComponent<Image>().
                                              sprite.texture;
        
        int width = (int)timelineSlider.value;
        
        for (int i = 0; i < timelineYPixelCount; i++)
        {
            Color pixelColor = timelineInspectorTexture.GetPixel(width, i);
            
            if (pixelColor.Equals(Color.red))
            {
                positionMarkerButton[i].GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                positionMarkerButton[i].GetComponent<Image>().color = Color.white;
            }
        }
    }
    
    // INFO : create 클릭 시 호출
    public void ShowObstacleTypeMenuGUI()
    {
        obstacleMenuGUI.SetActive(true);
        StartCoroutine(showObstacleTypeMenuDialog());
    }

    // INNER FUNCTION : ShowObstacleTypeMenuGUI()
    // INFO : 팝업에서 버튼이 선택되기를 대기
    private IEnumerator showObstacleTypeMenuDialog()
    {
        bool done = false;
        
        while (!done)
        {
            Button[] buttons = obstacleMenuGUI.GetComponentsInChildren<Button>();
            
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(delegate
                {
                    string buttonName = button.name;
                    
                    switch (buttonName)
                    {
                        case "Fragile":
                            currObstacleType = ObstacleType.FragileObstacle;
                            done = true;
                            break;
                        case "Static":
                            currObstacleType = ObstacleType.StaticObstacle;
                            done = true;
                            break;
                        case "Hole":
                            currObstacleType = ObstacleType.Hole;
                            done = true;
                            break;
                        default:
                            break;
                    }
                });
            }

            // 팝업 종료
            if (!createButtonToggle)
            {
                done = true;
            }
            
            yield return null;
        }
        
        obstacleMenuGUI.SetActive(false);

        // create버튼이 다시 눌린 경우 else 장애물이 선택된 경우
        if (createButtonToggle)
        {
            obstaclePositionMarker.SetActive(true);
        }
    }
    
    // INFO : 유틸리티, Song으로 노래를 성공적으로 불러오면 호출
    public void SetTimeline(AudioClip audioClip)
    {
        // TODO : 여기보다 다른 곳에 있어야 할 코드, 옮겨야 합니다
        // CAUTION : 그렇다고 지우지 마세요... 
        sliderAccessor.SongSelected = true;
        
        int width = (int)Math.Round(audioClip.length * 10);
        timelineSlider.maxValue = width;
    }
    
    // INFO : 유틸리티, timelineslider-value를 곡에서 사용하는 초단위로 변경
    public float GetSongCurrentRealTime()
    {
        return timelineSlider.value / 10f;
    }

    // INFO : 유틸리티, timelineslider-value를 곡에서 사용하는 초단위로 변경
    public void SetSongCurrentTime(float time)
    {
        int modifiedTime = (int)Math.Round(time * 10);
        timelineSlider.value = modifiedTime;
    }

    // INFO : 유틸리티, true는 선택됨, false는 선택X
    public void SetCreateButtonColor(bool flag)
    {
        if (flag)
        {
            createButton.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            createButton.GetComponent<Image>().color = Color.white;
        }
    }
    
    // INFO : create버튼 다시 누르면 호출
    public void ShutDownObstaclePositionMarker()
    {
        obstaclePositionMarker.SetActive(false);;
    }

    // INFO : 버튼이 클릭되면 index를 읽고 timelineinspector 이미지를 변경합니다.
    private void OnPositionMarkerButtonClicked(int index)
    {
        SongData songData = eventHandler.GetSongData();
        if (songData.source.clip == null)
        {
            Debug.Log("select song first!");
            return;
        }
        
        // 색 변경 코드
        Texture2D  timelineInspectorTexture = timelineSlider.
                                              GetComponent<RectTransform>().
                                              Find("Background").
                                              GetComponent<Image>().
                                              sprite.texture;
        
        Color color = new Color(0, 0, 0, 0.9f);
        
        if (positionMarkerButton[index].GetComponent<Image>().color == Color.white)
        {
            switch (currObstacleType)
            {
                case ObstacleType.FragileObstacle:
                    color = Color.red;
                    break;
            
                default:
                    // TODO : 여기 색깔 등록해주세요!
                    break;
            }
            
            // 버튼 클릭했으니 표시
            positionMarkerButton[index].GetComponent<Image>().color = Color.yellow;
        }
        else
        {   
            // 다시 클릭하면 지우기
            positionMarkerButton[index].GetComponent<Image>().color = Color.white;
        }
        
        timelineInspectorTexture.SetPixel((int)timelineSlider.value, index, color);
        timelineInspectorTexture.Apply();
    }
    
    // INFO : 유틸리티
    public Slider GetTimelineSlider()
    {
        return timelineSlider;
    }
}