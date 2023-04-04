/*
 *  업데이트  :  2023-04-04
 *  작성자    :  jiwon.
 *  파일 설명 :  맵 에디터의 전반적인 UI디자인 담당
 *               1. 노래 스크롤바
 *               2. 장애물 생성, 파일 입출력 버튼
 *               3. 맵UI
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class MapEditorUI : MonoBehaviour
{
    // INFO : 각 MapEditor클래스끼리 통신을 위한 객체
    private MapEditorEventHandler eventHandler;

    // INFO : 노래관련 
    private Scrollbar scrollbar;

    // INFO : 맵UI 관련
    private LineRenderer lineRenderer;
    private Material LineColorMat;
    private float sideLength;

    // INFO : 버튼UI 관련
    private Button saveFileButton;
    private Button openFileButton;
    private Button loadFileButton;
    private Button previewButton;
    private Button[] createObstacle;
    

    void Awake()
    {
        eventHandler = GetComponent<MapEditorEventHandler>();

        scrollbar = null;

        lineRenderer = GetComponent<LineRenderer>();
        LineColorMat = new Material(Shader.Find("Unlit/Texture"));
        sideLength = 7.5f;

        
    }

    void Start()
    {
        renderMapUI();
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
