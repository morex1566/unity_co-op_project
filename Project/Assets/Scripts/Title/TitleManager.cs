using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


namespace Title
{
    public enum BackgroundTexture
    {
        Title
    }
    
    public class TitleManager : MonoBehaviour
    {
        public static TitleManager Instance = null;

        [Header("Dependencies")] 
        [Space(5)]
        [SerializeField] private Texture lobbyBackgroundTexture;

        [Header("Lobby Setting")] 
        [Space(5)] 
        [SerializeField] private float backgroundYMoveLimit;
        [SerializeField] private float backgroundXMoveLimit;
        [SerializeField] private float backgroundMoveSpeed;
        
        

        public Texture LobbyBackgroundTexture => lobbyBackgroundTexture;
        public float BackgroundYMoveLimit => backgroundYMoveLimit;
        public float BackgroundXMoveLimit => backgroundXMoveLimit;
        public float BackgroundMoveSpeed => backgroundMoveSpeed;
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

        private void Update()
        {
            
        }
    }
}