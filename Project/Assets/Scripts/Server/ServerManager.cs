/*
 *  설명 : AWS Lambda와 통신합니다.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Utility;

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
        AddRank,
        LoadRank
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
        public int statusCode;
        public string msg;
    }

    public struct RankingPacket
    {
        public string UserName;
        public string SongName;
        public string Score;
    }
    
    public class ServerManager : MonoBehaviour
    {
        [Header("종속성")] [Space(5)] 
        
        [Tooltip("Amazon Lambda를 trigger하는 gatewayURL")]
        // gatewayURL 입력값 :
        // EventType=Ranking&CommandType=CommandTypeInput&UserName=UserNameInput&SongName=SongNameInput&Score=ScoreInput
        // EventType=Account&CommandType=CommandTypeInput&UserName=UserNameInput&ID=IDInput&Password=PasswordInput
        [SerializeField] private string gatewayURL;


        public static ServerManager Instance;

        private readonly Dictionary<AccountCommand, string> _accountCommandMap = new Dictionary<AccountCommand, string>()
        {
            {AccountCommand.Login, "Login"},
            {AccountCommand.Register, "Register"},
        };
        
        private readonly Dictionary<RankingCommand, string> _rankingCommandMap = new Dictionary<RankingCommand, string>()
        {
            {RankingCommand.AddRank, "AddRank"},
            {RankingCommand.LoadRank, "LoadRank"}
        };

        public IEnumerator Login(string userName, string id, string password, Action<bool> callback)
        {
            bool result = false;
            
            AlarmPopupManager.EnqueueAlarm("로그인 중입니다...", null, null, null);
            yield return StartCoroutine(serverRequest(AccountCommand.Login, userName, id, password, value =>
            {
                // 성공
                if (value >= 500)
                {
                    result = true;
                    AlarmPopupManager.EnqueueAlarm("로그인!", null, null, null);
                    callback?.Invoke(true);
                }
                // 실패
                else
                {
                    result = false;
                    AlarmPopupManager.EnqueueAlarm("실패ㅠㅠ, 아이디와 비밀번호를 확인해주세요", null, null, null);
                    callback?.Invoke(false);
                }
            }));

            yield return result;
        }
        
        public void Register(string userName, string id, string password)
        {
            AlarmPopupManager.EnqueueAlarm("계정을 생성중입니다...", null, null, null);
            StartCoroutine(serverRequest(AccountCommand.Register, userName, id, password, value =>
            {
                // 성공
                if (value >= 500)
                {
                    AlarmPopupManager.EnqueueAlarm("생성 완료!", null, null, null);
                }
                // 실패
                else
                {
                    AlarmPopupManager.EnqueueAlarm("이미 존재하는 계정...", null, null, null);
                }
            }));
        }

        public void AddRank(string userName, string songName, string score)
        {
            AlarmPopupManager.EnqueueAlarm("점수를 등록중입니다...", null, null, null);

        }

        public void LoadRank(string songName)
        {
            StartCoroutine(serverRequest(RankingCommand.LoadRank, "Test_User", songName, "10"));
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private IEnumerator serverRequest(AccountCommand command, string userName, string id, string password, Action<int> callback)
        {
            WWWForm body = new WWWForm();
            body.AddField("EventType", "Account");
            body.AddField("CommandType", _accountCommandMap.TryGetValue(command, out string value) ? value : null);
            body.AddField("UserName", userName);
            body.AddField("ID", id);
            body.AddField("Password", password);

            using UnityWebRequest request = UnityWebRequest.Post(gatewayURL, body);
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    foreach (KeyValuePair<string,string> valuePair in request.GetResponseHeaders())
                    {
                        if (valuePair.Key == "statusCode")
                        {
                            callback?.Invoke(Int32.Parse(valuePair.Value));
                        }
                    }
                }
            }
        }

        private IEnumerator serverRequest(RankingCommand command, string userName, string songName, string score = "10")
        {
            WWWForm body = new WWWForm();
            body.AddField("EventType", "Ranking");
            body.AddField("CommandType", _rankingCommandMap.TryGetValue(command, out string value) ? value : null);
            body.AddField("UserName", userName);
            body.AddField("SongName", songName);
            body.AddField("Score", score);
            
            using UnityWebRequest request = UnityWebRequest.Post(gatewayURL, body);
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    string responseText = request.downloadHandler.text;
                    
                    Debug.Log(responseText);
                    
                    // JSON 응답 데이터를 C# 객체로 Deserialize
                    if (responseText != null)
                    {
                        RankingPacket[] responseData = JsonUtility.FromJson<RankingPacket[]>(responseText);

                        // C# 객체를 사용하여 Unity에서 처리
                        foreach (RankingPacket data in responseData)
                        {
                            Debug.Log("UserName: " + data.UserName);
                            Debug.Log("SongName: " + data.SongName);
                            Debug.Log("Score: " + data.Score);
                        }
                    }
                }
            }
        }
    }
}
