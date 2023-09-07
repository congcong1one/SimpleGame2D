using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _PlayerState
{
    protected PlayerMovement player;
    bool IsEntered { get; }
    public _PlayerState(PlayerMovement player)
    {
        this.player = player;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }



}
