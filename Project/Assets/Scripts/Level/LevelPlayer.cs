
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    [Header("Dependencies")] 
    [Space(5)] 
    [SerializeField] private GameObject equipment;
    
    [Header("Player Setting")] 
    [Space(5)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int health;
    
    private PlayerEquipment _equipmentComponent;
    private PlayerMovement _movementComponent;
    private PlayerAction _actionComponent;

    private void Awake()
    {
        _equipmentComponent = new PlayerEquipment();
        _movementComponent = new PlayerMovement();
        _actionComponent = new PlayerAction();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _movementComponent.HandleInput();
        _actionComponent.HandleInput();
        
        _movementComponent.Update();
        _actionComponent.Update();
    }
}
