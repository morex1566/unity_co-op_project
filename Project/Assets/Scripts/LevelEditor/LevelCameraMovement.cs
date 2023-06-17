/*
 *  파일 설명   : 플레이어를 기준, InputSetting의 키를 입력하면, 카메라 시점을 변경.
 */

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


public class LevelCameraMovement : MonoBehaviour
{
    enum PointOfViewState
    {
        Left,
        Right
    }

    // CAUTION : 축의 + -는 앙페
    enum MovementAxisState
    {
        X,
        Y,
        Z
    }

    [SerializeField] private Animator eventAnim;
    [SerializeField] private TextMeshProUGUI msg;
    [SerializeField] private float translationSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject player;
    [FormerlySerializedAs("baseAxis")] [SerializeField] private MovementAxisState baseAxisState;

    [SerializeField] private Camera cam;

    private PointOfViewState _baseState;
    private PointOfViewState _currentState;
    
    private Vector3 _positionOffset;

    // INFO : 'PovState.Left'에 관한, '시점정보 변수'.
    private Vector3     _leftPovPosition;
    private Vector3     _leftPovRotation;

    // INFO : 'PovState.Right'에 관한, '시점정보 변수'.
    private Vector3     _rightPovPosition;
    private Vector3     _rightPovRotation;

    // INFO : 장애물 상호작용 시, '화면 흔들림 계수'.

    public static Vector3 VectorAbs(Vector3 vector)
    {
        return new Vector3(MathF.Abs(vector.x), Mathf.Abs(vector.y), MathF.Abs(vector.z));
    }

    public static Vector3 RotationAbs(Vector3 rotation)
    {
        return new Vector3(MathF.Abs(rotation.x), Mathf.Abs(rotation.y), MathF.Abs(rotation.z));
    }

    //**************************************** 유니티 Loop 함수들 ****************************************//
    void Start()
    {
        Vector3 revertedRotation = transform.rotation.eulerAngles;
        
        // ACTION : player와 camera간의 offset, Inspector에서 선택한 축을 기준으로 반전되는 위치, 회전값 저장
        switch (baseAxisState)
        {
            case MovementAxisState.X:
                _currentState = (player.transform.position.z > transform.position.z) ? PointOfViewState.Right : PointOfViewState.Left;
                
                _positionOffset = VectorAbs(transform.position - player.transform.position);
                _positionOffset.x = 0;
                _positionOffset.y = 0;

                switch (_currentState)
                {
                    case PointOfViewState.Left:
                        _leftPovPosition = new Vector3(transform.position.x, 
                            transform.position.y,
                            player.transform.position.z + _positionOffset.z);

                        _rightPovPosition = new Vector3(transform.position.x, 
                            transform.position.y,
                            player.transform.position.z - _positionOffset.z);

                        _leftPovRotation = transform.rotation.eulerAngles;

                        revertedRotation.x *= -1;
                        revertedRotation.z *= -1;

                        _rightPovRotation = revertedRotation;

                        break;
                    case PointOfViewState.Right:
                        _rightPovPosition = new Vector3(transform.position.x, 
                            transform.position.y,
                            player.transform.position.z - _positionOffset.z);
                        _leftPovPosition = new Vector3(transform.position.x, 
                            transform.position.y,
                            player.transform.position.z + _positionOffset.z);

                        _rightPovRotation = transform.rotation.eulerAngles;

                        revertedRotation.x *= -1;
                        revertedRotation.z *= -1;

                        _leftPovRotation = revertedRotation;
                        break;
                }

                break;
            
            case MovementAxisState.Y:
                _currentState = (player.transform.position.x > transform.position.x) ? PointOfViewState.Right : PointOfViewState.Left;
                
                _positionOffset = VectorAbs(transform.position - player.transform.position);
                _positionOffset.y = 0;
                _positionOffset.z = 0;

                switch (_currentState)
                {
                    case PointOfViewState.Right:
                        _rightPovPosition = new Vector3(player.transform.position.x - _positionOffset.x,
                            transform.position.y,
                            transform.position.z);
                            
                        _leftPovPosition = new Vector3(player.transform.position.x + _positionOffset.x,
                            transform.position.y,
                            transform.position.z);
                        
                        _rightPovRotation = transform.rotation.eulerAngles;
                        
                        revertedRotation.y *= -1;
                        revertedRotation.z *= -1;

                        _leftPovRotation = revertedRotation;

                        break;
                    case PointOfViewState.Left:
                        _leftPovPosition = new Vector3(player.transform.position.x + _positionOffset.x,
                            transform.position.y,
                            transform.position.z);
                        _rightPovPosition = new Vector3(player.transform.position.x - _positionOffset.x,
                            transform.position.y,
                            transform.position.z);
                        
                        _leftPovRotation = transform.rotation.eulerAngles;
                        
                        revertedRotation.y *= -1;
                        revertedRotation.z *= -1;

                        _rightPovRotation = revertedRotation;
                        break;
                }

                break;
            case MovementAxisState.Z:
                _currentState = (player.transform.position.y < transform.position.y) ? PointOfViewState.Right : PointOfViewState.Left;
                
                _positionOffset = VectorAbs(transform.position - player.transform.position);
                _positionOffset.z = 0;
                _positionOffset.x = 0;

                
                switch (_currentState)
                {
                    case PointOfViewState.Left:
                        _leftPovPosition = new Vector3(transform.position.x,
                            player.transform.position.y + _positionOffset.y,
                            transform.position.z);
                        _rightPovPosition  = new Vector3(transform.position.x,
                            player.transform.position.y - _positionOffset.y,
                            transform.position.z);
            
                        _leftPovRotation = transform.rotation.eulerAngles;
            
                        revertedRotation.x *= -1;
                        revertedRotation.y *= -1;

                        _rightPovRotation = revertedRotation;
                        break;
                    case PointOfViewState.Right:
                        _rightPovPosition  = new Vector3(transform.position.x,
                            player.transform.position.y - _positionOffset.y,
                            transform.position.z);
                        _leftPovPosition  = new Vector3(transform.position.x,
                            player.transform.position.y + _positionOffset.y,
                            transform.position.z);
            
                        _rightPovRotation = transform.rotation.eulerAngles;
            
                        revertedRotation.x *= -1;
                        revertedRotation.y *= -1;

                        _leftPovRotation = revertedRotation;
                        break;
                }
                
                break;
        }
        
    }
    
    private void Update()
    {
        if (Input.GetKey(InputSetting.MoveLeft))
        {
            _currentState = PointOfViewState.Left;
        }

        if (Input.GetKey(InputSetting.MoveRight))
        {
            _currentState = PointOfViewState.Right;
        }
        
        
        switch (_currentState)
        {
            case PointOfViewState.Left:
                transform.position = Vector3.Lerp(transform.position,
                    _leftPovPosition,
                    translationSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(_leftPovRotation),
                    rotationSpeed * Time.deltaTime);
                break;
                
            case PointOfViewState.Right:
                transform.position = Vector3.Lerp(transform.position,
                    _rightPovPosition,
                    translationSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(_rightPovRotation),
                    rotationSpeed * Time.deltaTime);
                break;
        }
    }

    public void SpeedUpEvent()
    {
        msg.enabled = true;

        msg.text = "Speed Up";
        eventAnim.SetTrigger("SpeedUp");
    }

    private void SetSpeedUp()
    {
        msg.enabled = false;

        cam.fieldOfView = 140;
        
        Debug.Log("SetSpeedUp");
    }
    
    private void SetSpeedDown()
    {
        msg.enabled = false;

        cam.fieldOfView = 120;
        
        Debug.Log("SetSpeedDown");
    }

    private void MsgOff()
    {
        msg.enabled = false;
        
        Debug.Log("MsgOff");
    }

    public void SpeedDownEvent()
    {
        msg.enabled = true;

        msg.text = "Speed Down";
        eventAnim.SetTrigger("SpeedDown");
    }
    
    //**************************************** 유니티 Loop 함수들 ****************************************//
}