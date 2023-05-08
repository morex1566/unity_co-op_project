using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Title
{
    public class TitleLobbyMenuBar : MonoBehaviour, IPointerExitHandler
    {
        public static TitleLobbyMenuBar Instance = null;

        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject editButton;
        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject exitButton;
        
        private Animator _animator;
        private static readonly int Appear = Animator.StringToHash("Appear");
        private static readonly int Disappear = Animator.StringToHash("Disappear");
        private static readonly int Idle = Animator.StringToHash("Idle");

        private Action _destroyEventHandler;
        public Action DestroyEventHandler { set => _destroyEventHandler = value; }
        
        
        // TODO : 의존성 주입 사용 
        public Animator MenuBarAnimator => _animator;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _animator = GetComponent<Animator>();

            // 한국어 패치
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.MenuStartButtonString;
            editButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.MenuEditButtonString;
            optionButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.MenuOptionButtonString;
            exitButton.GetComponentInChildren<TextMeshProUGUI>().text = Sentence.MenuExitButtonString;
        }

        private void Start()
        {
            // 생성 애니메이션
            _animator.SetTrigger(Appear);
        }

        // Update is called once per frame
        void Update()
        {
            // 종료
            if (Input.GetKeyDown(InputSetting.ExitEvent))
            {
                StartCoroutine(destroy());
            }
        }
        
        // TODO : 종료 이벤트 트리거 연결
        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetTrigger(Idle);
        }

        // 파괴와 동시에 애니메이션 호출
        private IEnumerator destroy()
        {
            _animator.SetTrigger(Disappear);
            _destroyEventHandler?.Invoke();

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

            Instance = null;
            Destroy(gameObject);
        }
    }
}
