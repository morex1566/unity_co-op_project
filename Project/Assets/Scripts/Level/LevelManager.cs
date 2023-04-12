using System.Data;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private IGameManager _gameManagerManager;
    
    enum RotationState
    {
        None,
        Left,
        Right
    }
    
    [Header("Dependencies")]
    [Space(5)]
    [SerializeField] private GameObject levelPlatformSpawner;
    [SerializeField] private GameObject levelObstacleSpawner;

    [Header("Level Rotation Setting")]
    [Space(5)]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float inertiaWeight;
    [SerializeField] private float inertiaSpeed;

    private Vector3 _inertiaRotation;
    private RotationState _rotationState;
    private Vector3 _rotationAxisPivot;
    
    void Awake()
    {
        _rotationState = RotationState.None;
        _inertiaRotation = Vector3.zero;
        _rotationAxisPivot = Vector3.zero;
    }

    void Start()
    {
        _gameManagerManager = GameManager.Instance as IGameManager;
        _rotationAxisPivot = _gameManagerManager.GetCenterPointAtLevel();
    }

    // CAUTION : 회전축이 vector3.forward로 고정되어있습니다.
    // TODO : GameManager에서 회전축을 바꿀 경우 생각해서 Config
    // TODO : 보간 방식으로 미끄러지는 것 보다는 camera의 speed를 조절하는게 더 좋을듯
    // TODO : Rotation같은 작업이 꽤 길어지는데..., 로직 분할해야 할듯
    // ref : https://unity.com/kr/how-to/how-architect-code-your-project-scales#monobehaviors-regular-c-classes
    void Update()
    {
        // 오른쪽으로 회전
        if (Input.GetKey(InputSetting.moveRight))
        {
            _rotationState = RotationState.Right;
            transform.RotateAround(_rotationAxisPivot, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        
        // 왼쪽으로 회전
        if (Input.GetKey(InputSetting.moveLeft))
        {
            _rotationState = RotationState.Left;
            transform.RotateAround(_rotationAxisPivot, Vector3.forward, -rotationSpeed * Time.deltaTime);

        }

        // 입력X 회전상태 변경
        if (!Input.GetKey(InputSetting.moveLeft) && !Input.GetKey(InputSetting.moveRight))
        {
            _rotationState = RotationState.None;
        }
        
        // 회전 입력 멈추면 가중치에 따라 미끄러짐 효과 값 설정
        if (_rotationState != RotationState.None)
        {
            switch (_rotationState)
            {
                case RotationState.Left:
                    _inertiaRotation = transform.rotation.eulerAngles;
                    _inertiaRotation.z -= inertiaWeight;
                    break;
            
                case RotationState.Right:
                    _inertiaRotation = transform.rotation.eulerAngles;
                    _inertiaRotation.z += inertiaWeight;
                    break;
            }
            
        }
        // 미끄리짐 수행
        else
        {
            // Quaternion currentRotation = transform.rotation;
            // Quaternion targetRotation = Quaternion.Euler(_inertiaRotation);
            // Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, inertiaSpeed * Time.deltaTime);
            // float inertiaRotateAngle = Quaternion.Angle(currentRotation, newRotation);
            //
            // // RotationState.Left
            // if (transform.rotation.eulerAngles.z > _inertiaRotation.z)
            // {
            //     inertiaRotateAngle *= -1;
            // }
            // // RotationState.Right
            // else
            // {
            // }
            //
            // transform.RotateAround(_rotationAxisPivot, Vector3.forward, inertiaRotateAngle);
        }
        
        
    }
}
