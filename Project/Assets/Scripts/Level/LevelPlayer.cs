using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    [Header("Dependencies")] 
    [Space(5)] 
    [SerializeField] private GameObject rightEquipment;
    [SerializeField] private GameObject leftEquipment;
    [Tooltip("현재 플레이어의 발 위치. 플레이어에 상속된 Foot GameObject를 넣어주세요.")]
    [SerializeField] private GameObject foot;

    [Header("Player Setting")] 
    [Space(5)]
    [SerializeField] private int health;
    [SerializeField] private int jumpPower;
    
    // Unity Component들
    private Rigidbody _rigidbody;
    
    // 사용자 정의 Component들
    private PlayerEquipment _equipmentComponent;
    private PlayerMovement _movementComponent;
    private PlayerAction _actionComponent;

    public GameObject GetRightEquipment() { return rightEquipment; }
    public GameObject GetLeftEquipment() { return leftEquipment; }
    public ref Rigidbody GetRigidbody() { return ref _rigidbody; }
    public int JumpPower { get { return jumpPower; } }
    public Vector3 FootPosition { get { return foot.transform.position; } }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _equipmentComponent = new PlayerEquipment();
        _movementComponent = new PlayerMovement();
        _actionComponent = new PlayerAction();
    }

    void Update()
    {
        _movementComponent.HandleInput(this);
        _actionComponent.HandleInput(this);
        
        _movementComponent.Update(this);
        _actionComponent.Update(this);
    }
}
