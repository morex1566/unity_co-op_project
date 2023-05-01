using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Title
{
    public class TitleMenuBarPlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Dependencies
        private TitleLobbyMenuBar _menuBar;
        
        private static readonly int PeekPlay = Animator.StringToHash("PeekPlay");

        public void Start()
        {
            _menuBar = TitleLobbyMenuBar.Instance;
        }


        // TODO : 연결해주세요
        public void OnClick()
        {
            SceneManager.LoadScene("SelectMap");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _menuBar.MenuBarAnimator.SetTrigger(PeekPlay);
        }
        
        // TODO : 종료 이벤트 트리거 연결
        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}