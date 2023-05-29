using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Title
{
    public class TitleTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("Timer GameObject의 배경 Panel")]
        [SerializeField] private Image backgroundPanel;

        [Tooltip("Timer GameObject에 포인터가 Enter되면 생기는 Select효과 속도")] 
        [SerializeField] private float duration;
    
        [Tooltip("Timer GameObject에 포인터가 Enter되면 생기는 Select효과 알파값")]
        [Range(0f, 1f)]
        [SerializeField] private float alpha;
        
        [Tooltip("현재 시간")]
        [SerializeField] private TextMeshProUGUI time;

        public void OnPointerEnter(PointerEventData eventData)
        {
            // StartCoroutine(onFadeIn());
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            // StartCoroutine(onFadeOut());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        private void Update()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            time.text = currentTime.ToString(@"hh\:mm\:ss");
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
