using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField]
    Scrollbar scrollbar;
    [SerializeField]
    Transform[] circleContents;
    [SerializeField]
    float swipeTime = 0.2f;
    [SerializeField]
    float swipeDistance = 10.0f;

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
        scrollPageValues = new float[transform.childCount];
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        for(int i=0; i<scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        maxPage = transform.childCount;
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
   

    
}
