using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SliderAccessor : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public string valueText; // UI Text component to display the current slider value
    
    // Start is called before the first frame update
    private bool isHandleDown = false; // Flag to indicate whether the handle is being held down
    
    // INFO : 사용자 동작으로 생성하는 UI관련
    [SerializeField] private GameObject messageBox;
    private MessageBoxAccessor msgAccessor;
    
    private void Start()
    {
        msgAccessor = messageBox.GetComponent<MessageBoxAccessor>();
        slider = transform.GetComponent<Slider>();
        
        valueText = slider.value.ToString();
        slider.onValueChanged.AddListener(delegate { onValueChanged(); });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        messageBox.SetActive(true);
        msgAccessor.textmeshpro.text = valueText;
        isHandleDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        messageBox.SetActive(false);
        msgAccessor.textmeshpro.text = valueText;
        isHandleDown = false;
    }

    private void onValueChanged()
    {
        valueText = milisecToTime();
        msgAccessor.textmeshpro.text = valueText;
    }

    private string milisecToTime()
    {
        int count = (int)slider.value;
        string str_min, str_sec, str_milsec;

        int int_min = count / 600;
        if (int_min > 9)
        {
            str_min = int_min.ToString();
        }
        else
        {
            str_min = "0" + int_min.ToString();
        }
        count = count % 600;

        int int_sec = count / 10;
        if (int_sec > 9)
        {
            str_sec = int_sec.ToString();
        }
        else
        {
            str_sec = "0" + int_sec.ToString();
        }
        count = count % 10;

        str_milsec = count.ToString() + "0";

        return str_min + ":" + str_sec + ":" + str_milsec;
    }
}
