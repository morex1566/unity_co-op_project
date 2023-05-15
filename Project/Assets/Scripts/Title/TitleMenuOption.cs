using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Title
{
    public class TitleMenuOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject option;        
        
        // Dependencies
        private TitleLobbyMenuBar _menuBar;
        
        private static readonly int PeekOption = Animator.StringToHash("PeekOption");

        public void Awake()
        {
            
        }

        public void Start()
        {
            _menuBar = TitleLobbyMenuBar.Instance;
        }


        // TODO : 연결해주세요
        public void OnClick()
        {
            GameObject uiLayer = GameObject.FindWithTag("Popup");

            if (uiLayer)
            {
                Instantiate(option, uiLayer.transform);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _menuBar.MenuBarAnimator.SetTrigger(PeekOption);
        }
        
        // TODO : 종료 이벤트 트리거 연결
        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}