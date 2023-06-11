using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Title
{
    public class TitleUser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("Login GameObject의 배경 Panel")]
        [SerializeField] private Image backgroundPanel;

        [Tooltip("Login GameObject에 포인터가 Enter되면 생기는 Select효과 속도")] 
        [SerializeField] private float duration;
    
        [Tooltip("Login GameObject에 포인터가 Enter되면 생기는 Select효과 알파값")]
        [Range(0f, 1f)]
        [SerializeField] private float alpha;

        [Tooltip("Login 기능을 하는 GameObject")]
        [SerializeField] private GameObject account;
    
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(onFadeIn());
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(onFadeOut());
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            account.SetActive(account.activeSelf == false ? true : false);
        }

        private IEnumerator onFadeIn()
        {
            while (backgroundPanel.color.a < alpha)
            {
                Color color = backgroundPanel.color;
                color.a += Time.deltaTime / duration;

                backgroundPanel.color = color;

                yield return null;
            }
        }

        private IEnumerator onFadeOut()
        {
            while (backgroundPanel.color.a > 0)
            {
                Color color = backgroundPanel.color;
                color.a -= Time.deltaTime / duration;

                backgroundPanel.color = color;

                yield return null;
            }
        }
    }
}
