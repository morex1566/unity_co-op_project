using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Title
{
    public class TitleToolbar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("Toolbar의 Panel에 대한 애니메이터")]
        [SerializeField] private Animator animator;

        [Tooltip("Toolbar를 좀 더 강조하도록 배경을 어둡게 하는 GameObject")]
        [SerializeField] private GameObject focusPanel;

        /// <summary>
        /// Toolbar를 On할지 Off할지 설정합니다
        /// <remarks> toolbar의 setActive는 애니메이션에 event되어있습니다</remarks>
        /// </summary>
        /// <param name="toggle"></param>
        public void OnOff(bool toggle)
        {
            Action work = toggle ? () =>
            {
                gameObject.SetActive(true);
                animator.SetTrigger("Open");
            } : () =>
            {
                animator.SetTrigger("Close");
            };
        
            work.Invoke();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            focusPanel.GetComponent<TitleFocusPanel>().OnOff(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            focusPanel.GetComponent<TitleFocusPanel>().OnOff(false);
        }
        
        private void deactivate() => gameObject.SetActive(false);
    }
}
