using UnityEngine;
using Server;
using System;
using TMPro;
using Utility;

namespace Title
{
    public class TitleAccount : MonoBehaviour
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("ID Input Field")] 
        [SerializeField] private TMP_InputField id;

        [Tooltip("Password Input Field")]
        [SerializeField] private TMP_InputField password;

        [Tooltip("사용자 이름")] 
        [SerializeField] private TextMeshProUGUI userName;

        private void Start()
        {
            loadUserInfo();
        }

        public void OnLogin()
        {
            if (id.text.Length <= 3)
            {
                AlarmPopupManager.EnqueueAlarm("ID는 적어도 3자리 이상이여야 합니다", null, null, null);
                return;
            }

            if (password.text.Length <= 3)
            {
                AlarmPopupManager.EnqueueAlarm("Password는 적어도 3자리 이상이여야 합니다", null, null, null);
                return;
            }

            StartCoroutine(ServerManager.Instance.Login(id.text, id.text, password.text, value =>
            {
                if (value)
                {
                    userName.text = id.text;
                    AccountManager.ID = id.text;
                    AccountManager.Password = password.text;
                    
                    eraseInputField();
                }
            }));
        }
        public void OnRegister()
        {
            ServerManager.Instance.Register(id.text, id.text, password.text);
            
            eraseInputField();
        }

        public void OnLogOut()
        {
            AccountManager.ID = null;
            AccountManager.Password = null;
            userName.text = AccountManager.Default;
            
            AlarmPopupManager.EnqueueAlarm("로그아웃", null, null, null);

        }

        private void eraseInputField()
        {
            id.text = "";
            password.text = "";
        }

        private void loadUserInfo()
        {
            userName.text = AccountManager.ID == null ? AccountManager.Default : AccountManager.ID;
        }
    }
}