using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MapEditorSlider : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{
    private MapEditorEventHandler eventHandler;
    
    public Slider slider;
    public string valueText;
    private bool songSelected;
    
    public bool SongSelected { get { return songSelected; } set { songSelected = value; } }
    
    // INFO : 사용자 동작으로 생성하는 UI관련
    [SerializeField] private GameObject messageBox;
    private MapEditorMessageBox msgAccessor;

    private void Awake()
    {
        eventHandler = GameObject.Find("EventManager").GetComponent<MapEditorEventHandler>();
        msgAccessor = messageBox.GetComponent<MapEditorMessageBox>();
        slider = transform.GetComponent<Slider>();
    }

    private void Start()
    {
        valueText = slider.value.ToString();
        slider.onValueChanged.AddListener(delegate { onValueChanged(); });
        songSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        messageBox.SetActive(true);
        msgAccessor.textmeshpro.text = valueText;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        messageBox.SetActive(false);
        msgAccessor.textmeshpro.text = valueText;
    }

    public void onValueChanged()
    {
        valueText = milisecToTime();
        msgAccessor.textmeshpro.text = valueText;

        if (songSelected == true)
        {
            eventHandler.UpdateTimelineInspector();
        }
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
