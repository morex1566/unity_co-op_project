using Server;
using UnityEngine;

namespace Utility
{
    public class CustomJsonParser : MonoBehaviour
    {
        public static RankingPackets[] ParseRankingPackets(string jsonString)
        {
            jsonString = jsonString.Trim('[', ']'); // 대괄호 제거
            string[] jsonItems = jsonString.Split(new string[] { "], [" }, System.StringSplitOptions.RemoveEmptyEntries); // 각 아이템 추출

            RankingPackets[] rankingPackets = new RankingPackets[jsonItems.Length];

            for (int i = 0; i < jsonItems.Length; i++)
            {
                string[] jsonValues = jsonItems[i].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries); // 각 값 추출

                if (jsonValues.Length >= 3)
                {
                    RankingPackets packet = new RankingPackets();
                    packet.username = jsonValues[0].Trim('\"');
                    packet.songname = jsonValues[1].Trim('\"');
                    packet.score = jsonValues[2].Trim('\"', ']'); // ] 문자 제거
                    packet.score = packet.score.Replace("\"", "");
                    rankingPackets[i] = packet;
                }
                else
                {
                    
                    return null;
                }
            }

            return rankingPackets;
        }
    }
}
