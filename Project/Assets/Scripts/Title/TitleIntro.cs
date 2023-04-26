using TMPro;
using UnityEngine;

namespace Title
{
    public class TitleIntro : MonoBehaviour
    {
        private string _recommendUsingHeadphoneString = "최상의 경험을 위해 헤드폰 사용을 권장합니다";
        private string _accessArchieveString = "아카이브에 접속중";
    
        private TextMeshProUGUI _recommendUsingHeadphone;
        private TextMeshProUGUI _accessArchieve;

        private void Awake()
        {
            _recommendUsingHeadphone = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            _accessArchieve = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _recommendUsingHeadphone.text = _recommendUsingHeadphoneString;
            _accessArchieve.text = _accessArchieveString;
        }
    }
}