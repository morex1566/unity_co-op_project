/*
 *  설명 : AlarmUI 생성 및 관리
 *  기능 :
 *  1. AlarmUI 생성 및 삭제
 *  2. AlarmUI의 움직임 제어
 */


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Utility
{
    public class AlarmPopupManager : MonoBehaviour
    {
        [Header("내부 종속성")] [Space(5)]
        
        [Tooltip("AlarmUI 객체")]
        [SerializeField] private GameObject alarmUI;
        
        [Tooltip("팝업이 생기는 기준점")]
        [SerializeField] private Transform pivot;
        
        [Tooltip("팝업의 수명")]
        [SerializeField] private float lifespan;

        [Tooltip("팝업 사이의 y축 간격")] 
        [SerializeField] private float offset;

        private static Queue<GameObject> _alarmUIs;
        private static AlarmPopupManager _instance;
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            _alarmUIs = new Queue<GameObject>();
        }
        
        /// <summary>
        /// 알림 팝업을 현재 Scene에 추가합니다.
        /// <param name="text">알림 메시지</param>
        /// <param name="icon">팝업창 왼쪽의 이미지 아이콘</param>
        /// <param name="startEvent">알림 팝업 시작 시, 호출되는 이벤트</param>
        /// <param name="endEvent">알림 팝업 종료 시, 호출되는 이벤트</param>
        /// </summary>
        public static void EnqueueAlarm(string text, GameObject icon, Action startEvent, Action endEvent)
        {

            GameObject popup = Instantiate(_instance.alarmUI, _instance.pivot);
            {
                // 알림 팝업 정보들 초기화
                AlarmUI popupInfo = popup.GetComponent<AlarmUI>();
                {
                    popupInfo.Text = text;
                    popupInfo.Icon = icon;
                    popupInfo.Lifespan = _instance.lifespan;
                    popupInfo.StartEventHandler += startEvent;
                    popupInfo.EndEventHandler += endEvent;
                }
                
                // 위치값 변경
                Vector3 popupPos = popup.transform.position;
                {
                    if (_alarmUIs.Count != 0)
                    {
                        popupPos.y -= _instance.alarmUI.GetComponent<RectTransform>().rect.height * _alarmUIs.Count;
                        popupPos.y -= _instance.offset * _alarmUIs.Count;
                    }

                    popup.transform.position = popupPos;
                }
                
                
                // 알림 리스트에 추가
                _alarmUIs.Enqueue(popup);
            }
        }

        /// <summary>
        /// 알림 팝업의 가장 앞에 있는 팝업을 큐에서 제거합니다
        /// <remarks> GameObject는 Destroy된다고 해서 Queue에서 빠지지 않기 때문에 수동으로 제거</remarks>
        /// </summary>
        public static void DequeueAlarm()
        {
            _alarmUIs.Dequeue();
        }
    }
}
