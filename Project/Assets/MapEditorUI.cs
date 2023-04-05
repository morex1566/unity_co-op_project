/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 전반적인 UI디자인 담당
 *               1. 노래 스크롤바
 *               2. 장애물 생성, 파일 입출력 버튼
 *               3. 맵UI
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class MapEditorUI : MonoBehaviour
{
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

    // INFO : 노래관련 
    [SerializeField] private Scrollbar songTimeLineScrollbar;
    [SerializeField] private GameObject songName;

    void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
        lineRenderer = GetComponent<LineRenderer>();
        LineColorMat = new Material(Shader.Find("Unlit/Texture"));
        sideLength = 7.5f;
    }

    void Start()
    {
        renderMapUI();

        saveFileButton.onClick.AddListener(eventHandler.OnSaveClicked);
        loadFileButton.onClick.AddListener(eventHandler.OnLoadClicked);
        playButton.onClick.AddListener(eventHandler.OnPlayClicked);
        stopButton.onClick.AddListener(eventHandler.OnStopClicked);
        createObstacleButton.onClick.AddListener(eventHandler.OnCreateClicked);
        songButton.onClick.AddListener(eventHandler.OnSongClicked);
    }

    private void renderMapUI()
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
}
