using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utility
{
    public class ExitMenu : MonoBehaviour
    {
        public static ExitMenu Instance = null;
        
        [Header("Dependencies")] 
        [Space(5)] 
        [SerializeField] private GameObject resumeButton;
        [SerializeField] private GameObject exitButton;
        [SerializeField] private TextMeshProUGUI msg;

        private Slider _gauge;

        public delegate void ExitEvent();
        public event ExitEvent ExitEventHandler;
        
        private void Awake()
        {
            // 싱글톤
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            _gauge = exitButton.transform.GetChild(1).GetComponent<Slider>();
            resumeButton.GetComponent<ExitMenuResumeButton>().ResumeButtonClickHandler += OnResumeButtonClick;
            exitButton.GetComponent<ExitMenuExitButton>().ExitButtonDownHandler += OnExitButtonDown;
            exitButton.GetComponent<ExitMenuExitButton>().ExitButtonUpHandler += OnExitButtonUp;

            
            resumeButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.ExitMenuResume;
            exitButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.ExitMenuExit;
            msg.text = Sentence.ExitMenuInformation;
        }

        private void OnExitButtonDown()
        {
            _gauge.value += Time.deltaTime;
            
            // 다 채워지면 이벤트 발생
            if (_gauge.value >= _gauge.maxValue - 0.1f)
            {
                ExitEventHandler.Invoke();
            }
        }

        private void OnExitButtonUp()
        {
            _gauge.value = _gauge.minValue;
        }

        private void OnResumeButtonClick()
        {
            Destroy(gameObject);
        }
    }
}
