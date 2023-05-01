using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    public class ExitMenuExitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action ExitButtonDownHandler;
        public event Action ExitButtonUpHandler;

        private bool isPressed = false;
        private void Update()
        {
            if (isPressed)
            {
                ExitButtonDownHandler.Invoke();
            }
            else
            {
                ExitButtonUpHandler.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }
    }
}