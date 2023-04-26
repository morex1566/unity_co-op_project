using System.Collections;
using TMPro;
using UnityEngine;

public class TitleIntroWelcome : MonoBehaviour
{
    private string _welcomeString = "환영합니다, 비트러너.";
    private TextMeshProUGUI _welcome;

    private void Awake()
    {
        _welcome = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }
    
    private IEnumerator drawStringProcedural()
    {
        for (int i = 0; i < _welcomeString.Length; i++)
        {
            _welcome.text += _welcomeString[i];

            if (_welcome.text.Length == 6)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
