using UnityEngine;

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
        [SerializeField] private AudioSource audioSource;

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
        public AudioSource AudioSource => audioSource;
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

        public void OnMusicStart()
        {
            audioSource.Play();
        }

        public void OnMusicStop()
        {
            audioSource.Stop();
        }
    }
}