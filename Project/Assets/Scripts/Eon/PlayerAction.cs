using LevelPlayerAction;
using UnityEngine;

public class PlayerAction
{
    private PlayerActionState _state;

    public PlayerAction()
    {
        _state = new Idle();
    }

    public void HandleInput()
    {
        _state.HandleInput(ref _state);
    }

    public void Update()
    {
        _state.Update(ref _state);
    }
}