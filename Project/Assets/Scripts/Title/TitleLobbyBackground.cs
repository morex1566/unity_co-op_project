using System;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    public class TitleLobbyBackground : MonoBehaviour
    {
        private TitleManager _titleManager;

        // Dependencies
        private Texture             _backgroundTexture;
        private RectTransform       _backgroundTransform;
        
        
        private float _backgroundYMoveLimit;
        private float _backgroundXMoveLimit;
        private float _backgroundMoveSpeed;

        private Vector3 _backgroundTargetPos;
        
        // CAUTION : 수정하지 마세요!
        private Vector3 _pivotBackgroundPos;

        private void Awake()
        {
            GameObject background = transform.GetChild(0).gameObject;

            _backgroundTexture = background.GetComponent<RawImage>().texture;
            _backgroundTransform = background.GetComponent<RectTransform>();
        }

        private void Start()
        {
            _titleManager = TitleManager.Instance;

            _backgroundTexture = _titleManager.LobbyBackgroundTexture;
            _backgroundYMoveLimit = _titleManager.BackgroundYMoveLimit;
            _backgroundXMoveLimit = _titleManager.BackgroundXMoveLimit;
            _backgroundMoveSpeed = _titleManager.BackgroundMoveSpeed;
            
            _backgroundTargetPos = _backgroundTransform.position;
            _pivotBackgroundPos = _backgroundTargetPos;
        }

        private void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");
            
            
            // background texture를 움직입니다
            if (mouseX != 0)
            {
                // limit 지점까지만 움직일 수 있게 합니다
                if (Math.Abs((_backgroundTargetPos.x + _backgroundMoveSpeed * mouseX) - _pivotBackgroundPos.x) <= _backgroundXMoveLimit)
                {
                    _backgroundTargetPos.x += _backgroundMoveSpeed * mouseX;
                }
            }

            // background texture를 움직입니다
            if (mouseY != 0)
            {
                // limit 지점까지만 움직일 수 있게 합니다
                if (Math.Abs((_backgroundTargetPos.y + _backgroundMoveSpeed * mouseY) -_pivotBackgroundPos.y) <= _backgroundYMoveLimit)
                {
                    _backgroundTargetPos.y += _backgroundMoveSpeed * mouseY;
                }
            }
            
            // 계산한 값을 토대로 부드럽게 움직입니다
            Vector3 movePos = Vector3.Lerp(_backgroundTransform.position, _backgroundTargetPos, 0.5f);
            _backgroundTransform.position = movePos;
        }

    }
}