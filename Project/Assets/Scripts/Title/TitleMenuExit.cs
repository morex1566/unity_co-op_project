using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Title
{
    public class TitleMenuExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Dependencies
        private TitleLobbyMenuBar _menuBar;
        
        private static readonly int PeekExit = Animator.StringToHash("PeekExit");

        public void Start()
        {
            _menuBar = TitleLobbyMenuBar.Instance;
        }


        // TODO : 연결해주세요
        public void OnClick()
        {
            Application.Quit();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _menuBar.MenuBarAnimator.SetTrigger(PeekExit);
        }
        
        // TODO : 종료 이벤트 트리거 연결
        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}