using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ILevelManager _gameManager;
 
    enum RotationState
    {
        None,
        Left,
        Right
    }
    
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
        _gameManager = GameManager.Instance as ILevelManager;
        _rotationAxisPivot = _gameManager.GetCenterPointAtLevel();
    }

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
        switch (_rotationState)
        {
            case RotationState.None:
                break;
            
            case RotationState.Left:
                _inertiaRotation = transform.rotation.eulerAngles;
                _inertiaRotation.z -= inertiaWeight;
                break;
            
            case RotationState.Right:
                _inertiaRotation = transform.rotation.eulerAngles;
                _inertiaRotation.z += inertiaWeight;
                break;
        }
        
        // // 미끄러짐 효과
        // transform.rotation = Quaternion.Lerp(transform.rotation,
        //     Quaternion.Euler(_inertiaRotation),
        //     inertiaSpeed * Time.deltaTime);
        
    }
}
