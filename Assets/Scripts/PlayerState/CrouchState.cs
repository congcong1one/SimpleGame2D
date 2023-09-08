using Assets.Scripts.PlayerState.PlayerState;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.PlayerState
{
    public class CrouchState : _PlayerState
    {
        [Header("蹲下蓄力时间的最大值")]
        private readonly float crouchTimeNumber = 2;
        public CrouchState(PlayerMovement player) : base(player) { }



        public override void Update() 
        {
            if (player.latch.Idle > 0)
                player.currentState.AddState(new IdleState(player));

            if (player.isJump(this, null))
            {
                return;
            }

            //增加蹲下的蓄力时间
            if (player.crouchTime < crouchTimeNumber)
            {
                player.crouchTime += Time.deltaTime;
            }

            //松开蹲下键
            if (Input.GetButtonUp("Crouch"))
            {
                player.currentState.RemoveState(this);
            }
            


        }
        public override void Enter()
        {
            player.latch.Crouch--;
            player.speed *= player.crouchSpeed;

            //下蹲时，碰撞体y轴方向大小和位置的变换
            player.boxCollider.size = new Vector2(player.boxCollider.size.x, player.boxCollider.size.y * player.crouchY);
            player.boxCollider.offset = new Vector2(player.boxCollider.offset.x, player.boxCollider.offset.y * player.crouchY);
        }
        public override void Exit()
        {
            player.speed /= player.crouchSpeed;
            ////取消下蹲时，碰撞体y轴方向大小和位置的还原
            player.boxCollider.size = new Vector2(player.boxCollider.size.x, player.boxCollider.size.y / player.crouchY);
            player.boxCollider.offset = new Vector2(player.boxCollider.offset.x, player.boxCollider.offset.y / player.crouchY);
            player.latch.Crouch++;
        }

    }
}