using TMPro;
using UnityEngine;
using Utility;

namespace Title
{
    public class TitleLobby : MonoBehaviour
    {
        // Dependencies
        [SerializeField] private GameObject menuBar;
        private TitleManager        _titleManager;
        private TextMeshProUGUI     _version;
        private RectTransform       _logoTransform;
        private GameObject _logo;
        private Vector3             _logoPivotScale;

        private Vector3 _maxLogoScale;
        private float _logoBounceWeight;
        private float _logoBounceBPS;

        private float _intervalTime;
        private Animator _animator;
        
        private static readonly int AsideLogo = Animator.StringToHash("AsideLogo");
        private static readonly int CenterLogo = Animator.StringToHash("CenterLogo");

        private void Awake()
        {
            _logoTransform = transform.GetChild(1).GetComponent<RectTransform>();
            _logo = transform.GetChild(1).gameObject;
            _version = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _titleManager = TitleManager.Instance;

            _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            
            _version.text = VersionInfo.Current;
            _logoPivotScale = _logoTransform.localScale;

            _logoBounceWeight = _titleManager.LogoBounceWeight;
            _logoBounceBPS = _titleManager.LogoBounceBPS;

            _maxLogoScale = _logoPivotScale;
            _maxLogoScale.x += _logoBounceWeight;
            _maxLogoScale.y += _logoBounceWeight;
            _maxLogoScale.z += _logoBounceWeight;

            _intervalTime = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            // 노래가 켜져있으면 Logo를 바운스
            if (BackgroundProcess.Instance.IsPlaying())
            {
                onLogoBounce();
            }
        }

        private void onLogoBounce()
        {
            _intervalTime += Time.deltaTime * _logoBounceBPS;
            if (_intervalTime >= Mathf.PI) {
                _intervalTime = 0.0f;
            }

            float scale = _logoBounceWeight * Mathf.Sin(_intervalTime);

            Vector3 newScale = _logoPivotScale + new Vector3(scale, scale, scale);
            
            _logo.transform.localScale = newScale;
        }

        // logo의 button component에 등록되어 TitleLobbyMenuBar를 생성합니다.
        public void OnMenuCreate()
        {
            if (!TitleLobbyMenuBar.Instance)
            {
                GameObject menu = Instantiate(menuBar, transform, false);
                menu.GetComponent<TitleLobbyMenuBar>().DestroyEventHandler = OnMenuShutDown;
                menu.name = "Menu Bar";
            
                _animator.SetTrigger(AsideLogo);
            }
        }

        public void OnMenuShutDown()
        {
            _animator.SetTrigger(CenterLogo);
        }
    }
}
