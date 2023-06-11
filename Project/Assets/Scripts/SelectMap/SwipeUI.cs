/*
 *      파일 설명 : UI생성 및 입력 제어
 *      주의 사항 : LoadMapDatas() 함수는 항상 Awake()의 가장 처음 호출해주세요!
 */


using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [Header("Dependencies")]
    [Space(5)]
    [SerializeField] private GameObject imageCircle;
    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject tutorial;
    
    // stage가 생성될 곳
    [SerializeField] private GameObject stageContent;
    
    // imageCircle이 생성될 곳
    [SerializeField] private GameObject panelCircleContent;
    
    [SerializeField]
    Scrollbar scrollbar;
    private Transform[] circleContents;
    [SerializeField]
    float swipeTime = 0.2f;
    [SerializeField]
    float swipeDistance = 10.0f;

    private List<MapData> _maps = new List<MapData>();

    float[] scrollPageValues;
    float valueDistance = 0;
    int currentPage = 0;
    int maxPage = 0;
    float startTouchX;
    float endTouchX;
    bool isSwipeMode = false;
    float circleContentScale = 1.6f;

    private void Awake()
    {
        LoadMapDatas();
    }
    void Start()
    {
        SetScrollBarValue(0);
    }

    public void SetScrollBarValue(int index)
    {
        currentPage = index;
        scrollbar.value = scrollPageValues[index];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateCircleContent();
    }

    void UpdateInput()
    {
        if (isSwipeMode ==true) return;

        if(Input.GetMouseButtonDown(0))
        {
            startTouchX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endTouchX = Input.mousePosition.x;
            
            UpdateSwipe();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdateSwipe(true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateSwipe(false);
        }
        
    }

    void UpdateSwipe()
    {
        if(Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            StartCoroutine(OnSwipeOneStep(currentPage));
            
            return;
        }

        bool isLeft = startTouchX < endTouchX ? true : false;

        if(isLeft == true)
        {
            if (currentPage == 0) return;

            currentPage--;
        }
        else
        {
            if (currentPage == maxPage - 1) return;
            
            currentPage++;
        }

        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    void UpdateSwipe(bool isLeft)
    {
        if(isLeft == true)
        {
            if (currentPage == 0) return;

            currentPage--;
        }
        else
        {
            if (currentPage == maxPage - 1) return;
            
            currentPage++;
        }
        
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    IEnumerator OnSwipeOneStep(int index)
    {
        float start = scrollbar.value;
        float current = 0;
        float percent = 0;
        

        isSwipeMode = true;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / swipeTime;

            scrollbar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        isSwipeMode = false;
    }

    void UpdateCircleContent()
    {
        for(int i=0; i < scrollPageValues.Length; ++i)
        {
            circleContents[i].localScale = Vector2.one;
            circleContents[i].GetComponent<Image>().color = Color.white;

            if(scrollbar.value < scrollPageValues[i] + (valueDistance /2) &&
                scrollbar.value > scrollPageValues[i] - (valueDistance /2))
            {
                circleContents[i].localScale = Vector2.one * circleContentScale;
                circleContents[i].GetComponent<Image>().color = Color.black;
            }
        }
    }


    /// <summary>
    /// Streaming Asset에 만들어진 맵 파일들을 읽고, UI를 생성
    /// </summary>
    private void LoadMapDatas()
    {
        // 맵 파일 전체 불러오기
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Application.streamingAssetsPath);
            FileInfo[] files = dirInfo.GetFiles("*.txt");

            foreach (var file in files)
            {
                _maps.Add(new MapData(FileLoadManager.LoadMapData(file.Name)));
            }

            // 튜토리얼용
             maxPage = _maps.Count + 1;
            // maxPage = _maps.Count;
        }

        // UI생성
        {
            // index 생성
            circleContents = new Transform[maxPage];
            
            // 스크롤되는 페이지의 value값을 저장
            scrollPageValues = new float[maxPage];
            
           
            for(int i=0; i< maxPage; ++i)
            {
                // 스크롤바 설정
                valueDistance = 1f / (maxPage - 1f);

                scrollPageValues[i] = valueDistance * i;
            }
            
            
            // 인스턴싱
            for (int i = 0; i < maxPage; i++)
            {
                // 튜토리얼용
                if (i == 0)
                {
                    GameObject tutorialInstance =  Instantiate(tutorial, stageContent.transform);
                    GameObject circle = Instantiate(imageCircle, panelCircleContent.transform);
                    circleContents[i] = circle.transform;
                    
                    continue;
                }
                
                GameObject stageInstance =  Instantiate(stage, stageContent.transform);
                stageInstance.GetComponentInChildren<StageData>().MapData = _maps[i - 1];
                GameObject circleInstance = Instantiate(imageCircle, panelCircleContent.transform);
                circleContents[i] = circleInstance.transform;
            }
        }
    }

    public void left()
    {
        UpdateSwipe(true);
    }

    public void right()
    {
        UpdateSwipe(false);
    }
}
