using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    public class ExitMenuResumeButton : MonoBehaviour, IPointerDownHandler
    {
        public event Action ResumeButtonClickHandler;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ResumeButtonClickHandler.Invoke();
        }
    }
}
