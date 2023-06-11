using System;
using UnityEngine;
using Utility;

namespace Title
{
    public enum BackgroundTexture
    {
        Title
    }

    public enum AudioBandCount
    {
        ThirtySix = 36,
        Sixty = 60,
        Ninety = 90,
        OneHundredTwenty = 120
    }

    public enum AudioSamplerCount
    {
        SixtyFour = 64,
        TwoHundredTwentyFour = 256,
        FiveHundredTweleve = 512
    }

    public class TitleManager : MonoBehaviour
    {
        public static TitleManager Instance = null;

        [Header("Dependencies")] 
        [Space(5)]
        [SerializeField] private Texture lobbyBackgroundTexture;
        [SerializeField] private GameObject exitMenu;
        [SerializeField] private GameObject option;
        [SerializeField] private Canvas uiLayer;
        
        [Header("Lobby Setting")] 
        [Space(5)] 
        [SerializeField] private float backgroundYMoveLimit;
        [SerializeField] private float backgroundXMoveLimit;
        [SerializeField] private float backgroundMoveSpeed;
        [Tooltip("기본값은 0.1 입니다.")]
        [SerializeField] private float logoBounceWeight;
        [SerializeField] private float logoBounceBPS;
        [SerializeField] private AudioBandCount audioBandCount;
        [SerializeField] private AudioSamplerCount audioSamplerCount;
        [SerializeField] private float audioBandMaxScale;
        
        public Texture LobbyBackgroundTexture => lobbyBackgroundTexture;
        public float BackgroundYMoveLimit => backgroundYMoveLimit;
        public float BackgroundXMoveLimit => backgroundXMoveLimit;
        public float BackgroundMoveSpeed => backgroundMoveSpeed;
        public float LogoBounceWeight => logoBounceWeight;
        public float LogoBounceBPS => logoBounceBPS;
        public AudioBandCount AudioBandCount => audioBandCount;
        public AudioSamplerCount AudioSamplerCount => audioSamplerCount;
        public float AudioBandMaxScale => audioBandMaxScale;
        private void Awake()
        {
            // 싱글톤 인스턴싱
            bool result = instantiate();
            if (!result)
            {
                return;
            }
        }

        private void Start()
        {
            StartCoroutine(BackgroundProcess.Instance.MusicFadeIn(1.0f));
            
        }

        private void Update()
        {
            // 종료 메뉴틀 부릅니다.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnExitMenuCreate();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                AlarmPopupManager.EnqueueAlarm("테스트", null, null, null);
            }
        }

        private bool instantiate()
        {
            bool result = false;
            
            // 이미 있다면 인스턴싱 실패
            if (Instance == null)
            {
                Instance = this;
                result = true;
            }
            else if(Instance != this)
            {
                Destroy(gameObject);
                result = false;
            }

            return result;
        }
        
        private void OnExitMenuCreate()
        {
            if (!ExitMenu.Instance && !TitleLobbyMenuBar.Instance && !Option.Instance)
            {
                GameObject exit = Instantiate(exitMenu, uiLayer.transform, false);
                exit.GetComponent<ExitMenu>().ExitEventHandler += OnGameExit;
            }
        }
        
        private void OnGameExit()
        {
            Application.Quit();
        }
    }
}