using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // INFO : �ִϸ��̼� ���� ������ ���⿡!
    [SerializeField] private Animator _animator;


    // INFO : Message ����� ����, '���� ī�޶�'.
    [SerializeField] private GameObject _mainCamera;


    // INFO : ����Ű �޺� ���Ǻ���.
    // ex) DŰ ��Ÿ ��, ���ʺ���->�����ʺ���->���ʺ���->�����ɤ�....
    [SerializeField] private bool  _leftComboEnabled;
    [SerializeField] private bool  _rightComboEnabled;


    // INFO : 'KEY_EVENT'�� ����Ű�� �׿� ���� method�� ����, Start()���� ���ε�.
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



    //**************************************** ����Ƽ Loop �Լ��� ****************************************//

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

    //**************************************** ����Ƽ Loop �Լ��� ****************************************//

}