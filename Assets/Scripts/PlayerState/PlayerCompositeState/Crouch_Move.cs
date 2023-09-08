using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch_Move : _PlayerState
{
    public Crouch_Move(PlayerMovement player) : base(player) { }

    private List<_PlayerState> _PlayerStates = new List<_PlayerState>();

    public void AddState(_PlayerState state)
    {
        _PlayerStates.Add(state);
        state.Enter();
    }

    public void RemoveState(_PlayerState state)
    {
        _PlayerStates.Remove(state);
        state.Exit();
    }
}
