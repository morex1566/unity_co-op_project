using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
// ReSharper disable All

// TODO : 오브젝트 풀링으로 최적화 가능
public class HitPopup : MonoBehaviour
{
    [Header("내부 종속성")] [Space(5)] 
    
    [SerializeField] private TextMeshProUGUI text;

    
    [Header("설정")] [Space(5)] 
    
    [Tooltip("팝업창이 유지되는 시간")]
    [SerializeField] private float lifespan;
    [Tooltip("팝업창이 위로 움직이는 속도")]
    [SerializeField] private float speed;
    [Tooltip("팝업창이 생성되는 지점 가중치")] 
    [SerializeField] private Vector3 offset;
    [Tooltip("팝업창 애니메이터")]
    [SerializeField] private Animator animator;

    
    /// <summary>
    /// 팝업 UI를 Scene에 인스턴싱합니다.
    /// <param name="transform_">팝업이 생성되는 위치, 회전정보</param>
    /// <param name="text_">팝업의 내용</param>
    /// </summary>
    public void ShowPopup(Transform transform_, string text_)
    {
        text.text = text_;
        
        Instantiate(gameObject, transform_.position + offset, quaternion.identity);
    }

    private void Start()
    {
        StartCoroutine(textFadeOut());
        StartCoroutine(atDestroy(lifespan));
    }

    private void Update()
    {
        textMoveUp();
    }
    
    private IEnumerator atDestroy(float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime == 0f ? lifespan : waitTime);
     
        Debug.Log("done");
        
        Destroy(gameObject);
    }

    /// <summary>
    /// 팝업 UI가 위로 점차 올라가는 효과입니다.
    /// </summary>
    private void textMoveUp()
    {
        Vector3 nextPos = transform.position;
        {
            nextPos.y += speed * Time.deltaTime;
        }

        transform.position = nextPos;
    }

    /// <summary>
    /// 팝업 UI가 점차 사라지는 효과입니다.
    /// </summary>
    private IEnumerator textFadeOut()
    {
        yield return new WaitForSeconds(lifespan * 1 / 3);

        animator.SetTrigger("FadeOut");
    }
}
