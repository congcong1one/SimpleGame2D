using Assets.Scripts.PlayerState.PlayerState;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerState.PlayerCompositeState
{
    public class StateEnable 
    {
        public bool Idle;
        public bool Jump;
        public bool Move;
        public bool Crouch;

        public void Init()
        {
            Idle = false;
            Jump = false;
            Move = false;
            Crouch = false;
        }

    }


}