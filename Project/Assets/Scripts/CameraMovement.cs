using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    // INFO : 시점의 종류.
    enum PovState
    {
        Left,
        Right
    }
    [SerializeField] private PovState _povState;

    // INFO : 'PovState.Left'에 관한, '시점정보 변수'.
    private Vector3     _leftPovPosition;
    private Vector3     _leftPovRotation;

    // INFO : 'PovState.Right'에 관한, '시점정보 변수'.
    private Vector3     _rightPovPosition;
    private Vector3     _rightPovRotation;

    // INFO : 시점 변환 시, 카메라 전환과 관련되는 변수.
    [SerializeField] private float _conversionSpeed;
    [SerializeField] private bool _conversionDone;


    // INFO : 장애물 상호작용 시, '화면 흔들림 계수'.


    // INFO : 'PlayerMovement'의 콜백함수들 여기에!
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

    //**************************************** 유니티 Loop 함수들 ****************************************//
    void Awake()
    {
        // CAUTION : 시점의 기준은 '왼쪽', 변경 시 정의되지 않은 동작 위험.
        _povState = PovState.Left;
        _leftPovPosition = transform.position;
        _leftPovRotation = transform.rotation.eulerAngles;


        // INFO : 'PovState.Right'는, 'Left 시점의 Y축 반전'.
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
    //**************************************** 유니티 Loop 함수들 ****************************************//
}
