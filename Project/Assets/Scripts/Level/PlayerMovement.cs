using LevelPlayerMovement;

public class PlayerMovement
{
    private static Idle _idle;
    private static RightMove _rightMove;
    private static LeftMove _leftMove;
    private static Jump _jump;
    private static Slide _slide;

    private PlayerMovementState _state;

    public PlayerMovement()
    {
        _idle = new Idle();
        _rightMove = new RightMove();
        _leftMove = new LeftMove();
        _jump = new Jump();
        _slide = new Slide();

        _state = _idle;
    }

    public void HandleInput()
    {
        _state.HandleInput();
    }

    public void Update()
    {
        _state.Update();
    }
}