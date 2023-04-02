using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // INFO : 애니메이션 관련 정보들 여기에!
    [SerializeField] private Animator _animator;


    // INFO : Message 통신을 위한, '메인 카메라'.
    [SerializeField] private GameObject _mainCamera;


    // INFO : 공격키 콤보 조건변수.
    // ex) D키 연타 시, 왼쪽베기->오른쪽베기->왼쪽베기->오른쪼ㄱ....
    [SerializeField] private bool  _leftComboEnabled;
    [SerializeField] private bool  _rightComboEnabled;


    // INFO : 'KEY_EVENT'에 조작키와 그에 관한 method를 적고, Start()에서 바인딩.
    [SerializeField] private Dictionary<KeyCode, Action> _keyEvents;
    #region KEY_EVENT

    private void onLeftSlashDown()
    {
        if (!_leftComboEnabled)
        {
            _animator.SetTrigger("LEFT_SLASH1");
            _leftComboEnabled = true;
            _rightComboEnabled = false;
        }
        else
        {
            _animator.SetTrigger("LEFT_SLASH2");
            _leftComboEnabled = false;
            _rightComboEnabled = false;
        }
    }

    private void onRightSlashDown()
    {
        if (!_rightComboEnabled)
        {
            _animator.SetTrigger("RIGHT_SLASH1");
            _rightComboEnabled = true;
            _leftComboEnabled = false;
        }
        else
        {
            _animator.SetTrigger("RIGHT_SLASH2");
            _rightComboEnabled = false;
            _leftComboEnabled = false;
        }
    }

    private void onMoveLeft()
    {
        _mainCamera.gameObject.SendMessage("SetLeftPov");
    }

    private void onMoveRight()
    {
        _mainCamera.gameObject.SendMessage("SetRightPov");
    }

    #endregion



    //**************************************** 유니티 Loop 함수들 ****************************************//

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _mainCamera = GameObject.FindWithTag("MainCamera");

        _leftComboEnabled = false;
        _rightComboEnabled = false;

        _keyEvents = new Dictionary<KeyCode, Action>()
        {
            {InputSetting.leftSlash, onLeftSlashDown },
            {InputSetting.rightSlash, onRightSlashDown },
            {InputSetting.moveLeft, onMoveLeft},
            {InputSetting.moveRight, onMoveRight}
        };
    }

    void Update()
    {
        foreach(var keyEvent in _keyEvents)
        {
            if(Input.GetKeyDown(keyEvent.Key))
            {
                keyEvent.Value();
            }
        }
    }

    //**************************************** 유니티 Loop 함수들 ****************************************//

}