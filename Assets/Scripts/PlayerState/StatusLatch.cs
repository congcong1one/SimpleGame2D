using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PlayerState
{
    public class StatusLatch 
    {

        public uint Idle;
        public uint Jump;
        public uint Crouch;
        public uint Move;

        public readonly uint maxIdle = 1;
        public uint maxJump = 2;
        public readonly uint maxCrouch = 1;
        public readonly uint maxMove = 1;

        public void Init()
        {
            Idle = 1; Jump = 2; Crouch = 1; Move = 1;
        }

        public void idleInit()
        {
            Idle = maxIdle;
        }
        public void jumpInit()
        {
            Jump = maxJump;
        }
        public void crouchInit()
        {
            Crouch = maxCrouch;
        }
        public void moveInit()
        {
            Move = maxMove;
        }
    }
}