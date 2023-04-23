using LevelPlayerMovement;

public class PlayerMovement
{
    private PlayerMovementState _state;

    public PlayerMovement()
    {
        _state = new Idle();
    }

    public void HandleInput(LevelPlayer player)
    {
        _state.HandleInput(ref player, ref _state);
    }

    public void Update(LevelPlayer player)
    {
        _state.Update(ref player, ref _state);
    }
}