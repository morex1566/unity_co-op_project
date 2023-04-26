using System.Collections;
using UnityEngine;



namespace Title
{
    public enum BackgroundTexture
    {
        Intro,
        Title
    }
    
    public class TitleManager : MonoBehaviour
    {
        public static TitleManager Instance = null;

        [Header("Dependencies")] 
        [Space(5)]
        [SerializeField] private Texture introBackground;
        [SerializeField] private Texture titleBackground;

        [Header("Event Setting")] [Space(5)] 
        [SerializeField] private float flashDuration;

        private Texture[] _sceneTexture;
        
        public Texture IntroBackground => introBackground;
        public Texture TitleBackground => titleBackground;
        public float FlashDuration => flashDuration;
        
        private void Awake()
        {
            // 싱글톤 인스턴싱
            bool result = instantiate();
            if (!result)
            {
                return;
            }

            _sceneTexture = new Texture[2];
            _sceneTexture[(int)BackgroundTexture.Intro] = introBackground;
            _sceneTexture[(int)BackgroundTexture.Title] = titleBackground;
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
    }
}