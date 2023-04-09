using UnityEngine;



public class CameraMovement : MonoBehaviour
{
    enum PovState
    {
        Left,
        Right
    }
    [SerializeField] private PovState _povState;

    private Vector3     _leftPovPosition;
    private Vector3     _leftPovRotation;

    private Vector3     _rightPovPosition;
    private Vector3     _rightPovRotation;

    [SerializeField] private float _conversionSpeed;
    

    void Awake()
    {
        _povState = PovState.Left;
        _leftPovPosition = transform.position;
        _leftPovRotation = transform.rotation.eulerAngles;


        _rightPovPosition = transform.position;
        _rightPovPosition.x = -transform.position.x;

        _rightPovRotation = transform.rotation.eulerAngles;
        _rightPovRotation.y = -transform.rotation.eulerAngles.y;
        _rightPovRotation.z = -transform.rotation.eulerAngles.z;

        _conversionSpeed = 1.0f;
    }
    void Update()
    {
        InputUpdate();

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

    private void InputUpdate()
    {
        if (Input.GetKey(InputSetting.moveLeft))
        {
            _povState = PovState.Left;
        }

        if (Input.GetKey(InputSetting.moveRight))
        {
            _povState = PovState.Right;
        }
    }
}
