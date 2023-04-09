using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    // INFO : ������ ����.
    enum PovState
    {
        Left,
        Right
    }
    [SerializeField] private PovState _povState;

    // INFO : 'PovState.Left'�� ����, '�������� ����'.
    private Vector3     _leftPovPosition;
    private Vector3     _leftPovRotation;

    // INFO : 'PovState.Right'�� ����, '�������� ����'.
    private Vector3     _rightPovPosition;
    private Vector3     _rightPovRotation;

    // INFO : ���� ��ȯ ��, ī�޶� ��ȯ�� ���õǴ� ����.
    [SerializeField] private float _conversionSpeed;
    [SerializeField] private bool _conversionDone;


    // INFO : ��ֹ� ��ȣ�ۿ� ��, 'ȭ�� ��鸲 ���'.


    // INFO : 'PlayerMovement'�� �ݹ��Լ��� ���⿡!
    #region MSG_EVENT
    public void SetLeftPov()
    {
        _povState = PovState.Left;
    }

    public void SetRightPov()
    {
        _povState = PovState.Right;
    }
    #endregion

    //**************************************** ����Ƽ Loop �Լ��� ****************************************//
    void Awake()
    {
        // CAUTION : ������ ������ '����', ���� �� ���ǵ��� ���� ���� ����.
        _povState = PovState.Left;
        _leftPovPosition = transform.position;
        _leftPovRotation = transform.rotation.eulerAngles;


        // INFO : 'PovState.Right'��, 'Left ������ Y�� ����'.
        _rightPovPosition = transform.position;
        _rightPovPosition.x = -transform.position.x;

        _rightPovRotation = transform.rotation.eulerAngles;
        _rightPovRotation.y = -transform.rotation.eulerAngles.y;
        _rightPovRotation.z = -transform.rotation.eulerAngles.z;

        _conversionSpeed = 1.0f;
    }

    void Start()
    {
    }

    void Update()
    {
        switch (_povState)
        {
            case PovState.Left:
                transform.position = Vector3.Lerp(transform.position,
                                                  _leftPovPosition,
                                                  _conversionSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.Euler(_leftPovRotation),
                                                     _conversionSpeed * Time.deltaTime);
                break;
            case PovState.Right:
                transform.position = Vector3.Lerp(transform.position,
                                                  _rightPovPosition,
                                                  _conversionSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.Euler(_rightPovRotation),
                                                     _conversionSpeed * Time.deltaTime);
                break;
        }
    }
    //**************************************** ����Ƽ Loop �Լ��� ****************************************//
}
