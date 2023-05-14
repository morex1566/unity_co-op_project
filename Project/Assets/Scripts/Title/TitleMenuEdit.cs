using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Title
{
    public class TitleMenuEdit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Dependencies
        private TitleLobbyMenuBar _menuBar;
        
        private static readonly int PeekEdit = Animator.StringToHash("PeekEdit");

        public void Start()
        {
            _menuBar = TitleLobbyMenuBar.Instance;
        }


        // TODO : 연결해주세요
        public void OnClick()
        {
            BackgroundProcess.Instance.OnStopMusic();
            SceneManager.LoadScene("MapEditor");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _menuBar.MenuBarAnimator.SetTrigger(PeekEdit);
        }
        
        // TODO : 종료 이벤트 트리거 연결
        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
