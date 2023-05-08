using UnityEngine;

namespace Utility
{
    public class LoadingPage : MonoBehaviour
    {
        [SerializeField] private static GameObject self;
        public static LoadingPage Instance = null;
        
        private Animator _animator;
    
        private static readonly int FadeIn = Animator.StringToHash("FadeIn");
        private static readonly int FadeOut = Animator.StringToHash("FadeOut");

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
            
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            OnFadeIn();
        }

        // TODO : 이 함수에서 로딩이 끝났는지 확인
    

        public void OnFadeIn()
        {
            _animator.SetTrigger(FadeIn);
        }

        /// <remark> 아직 로딩에 대한 처리가 없어서, FadeIn Animation에서 트리거 됩니다 </remark>
        public void OnFadeOut()
        {
            _animator.SetTrigger(FadeOut);
        }

        /// <remark> FadeOut Animation에서 트리거 됩니다 </remark>
        private void destroy()
        {
            Destroy(gameObject);
        }
    }
}
