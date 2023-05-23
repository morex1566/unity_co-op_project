/*
 *  설명 : AWS Lambda와 통신합니다.
 */


using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

namespace Server
{
    /// <summary>
    /// <remarks> AWS Lambda와 일관성을 유지해야합니다!!! </remarks>
    /// </summary>
    public enum AccountCommand
    {
        Login,
        Register,
    }

    /// <summary>
    /// <remarks> AWS Lambda와 일관성을 유지해야합니다!!! </remarks>
    /// </summary>
    public enum RankingCommand
    {
        Add,
        Load
    }

    /// <summary>
    /// <remarks> AWS Lambda와 일관성을 유지해야합니다!!! </remarks>
    /// 에러 코드 : <br/>
    /// StatusCode == 401 : 로그인 실패, password나 id가 일치하지 않습니다. <br/>
    /// StatusCode == 402 : 사용자이름이 이미 존재합니다. <br/>
    /// StatusCode == 403 : 아이디가 이미 존재합니다. <br/>
    /// </summary>
    public struct AccountPacket
    {
        public string StatusCode;
        public string Msg;
    }

    public struct RankingPacket
    {
        public string StatusCode;
        public string Msg;
    }
    
    public class ServerManager : MonoBehaviour
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("Amazon Lambda를 trigger하는 gatewayURL")]
        // gatewayURL 입력값 :
        // EventType=Ranking&CommandType=CommandTypeInput&UserName=UserNameInput&SongName=SongNameInput&Score=ScoreInput
        // EventType=Account&CommandType=CommandTypeInput&UserName=UserNameInput&ID=IDInput&Password=PasswordInput
        [SerializeField] private string gatewayURL;


        private readonly Dictionary<AccountCommand, string> _accountCommandMap = new Dictionary<AccountCommand, string>()
        {
            {AccountCommand.Login, "Login"},
            {AccountCommand.Register, "Register"},
        };
        
        private readonly Dictionary<RankingCommand, string> _rankingCommandMap = new Dictionary<RankingCommand, string>()
        {
            {RankingCommand.Add, "Add"},
            {RankingCommand.Load, "Load"}
        };


        /// <summary>
        /// 서버에게 Request합니다.
        /// </summary>
        /// <param name="command">Request할 명령타입</param>
        public void ServerCommand(AccountCommand command, string userName, string id, string password)
            => StartCoroutine(serverRequest(command, userName, id, password));
        /// <summary>
        /// 서버에게 Request합니다.
        /// </summary>
        /// <param name="command">Request할 명령타입</param>
        // public void ServerCommand(RankingCommand command, string userName, int score) => StartCoroutine(serverRequest(command, userName, score));

        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            
        }
        
        private IEnumerator serverRequest(AccountCommand command, string userName, string id, string password)
        {
            WWWForm body = new WWWForm();
            body.AddField("EventType", "Account");
            body.AddField("CommandType", _accountCommandMap.TryGetValue(command, out string value) ? value : null);
            body.AddField("UserName", userName);
            body.AddField("ID", id);
            body.AddField("Password", password);

            UnityWebRequest request = UnityWebRequest.Post(gatewayURL, body);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AccountPacket packet = JsonUtility.FromJson<AccountPacket>(request.downloadHandler.text);
                Debug.Log(packet.StatusCode);
                Debug.Log(packet.Msg);
            }
        }

        // // TODO : 비동기 메서드마다 결과값이 다를텐데.. 이걸 해결하는 방법
        // private IEnumerator serverRequest(RankingCommand command, string userName, int score)
        // {
        //     WWWForm body = new WWWForm();
        //     body.AddField("EventType", "Ranking");
        //     body.AddField("CommandType", _rankingCommandMap.TryGetValue(command, out string value) ? value : null);
        //     
        // }
    }
}
