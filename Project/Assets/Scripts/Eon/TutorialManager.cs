using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public float arrowTime = 10f;
    public float fadeTime = 1f;

    float arrowClickTime;
    int actionCount = 0;

    int tutorialNum = 0;

    bool complete = false;

    bool left=false, right = false;

    public GameObject ObjectSpawner;
    public GameObject level;
    public GameObject end;

    string[] textList =
    {
        "←, → 키를 이용해 좌우로 움직이세요",
        "↑ 키를 이용해 점프하세요",
        "A, D버튼을 이용해 블록을 파괴하세요"
    };

    public TMP_Text tutoText;

    private void Start()
    {
        tutoText.text = textList[0];
    }
    void Update()
    {
        switch(tutorialNum)
        {
            case (0):
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    if (arrowClickTime < arrowTime)
                    {
                        arrowClickTime += Time.deltaTime;
                        Debug.Log(arrowClickTime);
                    }
                    else
                    {
                        if (!complete)
                            StartCoroutine(FadeOut());
                    }
                }
                break;

            case (1):
                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Debug.Log(actionCount);
                    actionCount++;
                    if(actionCount>0)
                    {
                        StartCoroutine(FadeOut(3));
                    }
                }
                break;

            case (2):
                ObjectSpawner.SetActive(true);
                level.transform.position = Vector3.zero;
                level.transform.rotation= Quaternion.Euler(0,0,0);
                level.GetComponent<LevelManager>().enabled = false;
                if (Input.GetKeyDown(KeyCode.A))
                    left = true;
                else if (Input.GetKeyDown(KeyCode.D))
                    right = true;

                if (left && right)
                    StartCoroutine(FadeOut());    
                break;

            case (3):
                end.SetActive(true);
                break;
                
        }
        
        
    }

    IEnumerator FadeOut(float delay = 0)
    {
        complete = true;
        tutoText.color = new Color(tutoText.color.r, tutoText.color.g, tutoText.color.b, 1);
        while(tutoText.color.a >0f)
        {
            tutoText.color = new Color(tutoText.color.r, tutoText.color.g, tutoText.color.b,
                tutoText.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }

        tutorialNum++;
        tutoText.text = textList[tutorialNum];
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        tutoText.color = new Color(tutoText.color.r, tutoText.color.g, tutoText.color.b, 0);


        while (tutoText.color.a < 1f)
        {
            tutoText.color = new Color(tutoText.color.r, tutoText.color.g, tutoText.color.b,
                tutoText.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }
        
        complete = false;
        
    }

    public void Retry()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
