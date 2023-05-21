using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class AlarmUI : MonoBehaviour
    {
        [Header("내부 종속성")] [Space(5)]
        
        [Tooltip("팝업 메세지")]
        [SerializeField] private TextMeshProUGUI text;
        public string Text 
        { 
            get => text.text;
            set => text.text = value;
        }
        
        [Tooltip("팝업 아이콘 생기는 기준점")] 
        [SerializeField] private GameObject iconPivot;

        [SerializeField] private Animator animator;

        public GameObject Icon { get; set; }
        public Action StartEventHandler { get; set; }
        public Action EndEventHandler { get; set; }
        public float Lifespan { get; set; }
        

        // Start is called before the first frame update
        private void Start()
        {
            // icon 생성
            if(Icon != null)
            {
               Instantiate(Icon, iconPivot.transform);
            }
            
            StartEventHandler?.Invoke();

            StartCoroutine(atDestroy());
            
            animator.SetTrigger("Appear");
        }
        
        // TODO : 애니메이션의 타이머와 생명주기를 맞출 수는 없을까?
        // TODO : 애니메이션의 위치이동, 특히 UI에 관련해서 해상도에 따른 자동 조절?
        private IEnumerator atDestroy(float waitTime = 0f)
        {
            yield return new WaitForSeconds(waitTime == 0f ? Lifespan / 3 * 2 : waitTime);
            
            animator.SetTrigger("Disappear");
            
            yield return new WaitForSeconds(waitTime == 0f ? Lifespan / 3 : waitTime);
        
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EndEventHandler?.Invoke();
            AlarmPopupManager.DequeueAlarm();
        }
    }
}