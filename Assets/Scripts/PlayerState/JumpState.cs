using Assets.Scripts.PlayerState.PlayerState;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PlayerState
{
    public class JumpState : _PlayerState
    {
        [Header("起跳后与再次起跳直接的间隔时间，防止接触地面时连续起跳")]
        private float timer;

        [Header("跳跃的力")]
        private float jumpForce = 20f;

        [Header("跳跃的力与时间乘积所要乘的系数，以确保数值正确")]
        private readonly float jumpNumber = 0.25f;

        RaycastHit2D hit;
        public JumpState(PlayerMovement player) : base(player) { }
        public JumpState(PlayerMovement player, bool isCharge) : base(player)
        {
            if (isCharge)
            {
                jumpForce += jumpForce * player.crouchTime * jumpNumber;
            }
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            hit = Physics2D.Raycast(player.transform.position, Vector2.down, 0.2f, 1<<8);
            
            if(hit.collider != null && hit.collider.tag == "Ground" && timer > 0.8)
            {
                //退出跳跃状态时，移除蹲下蓄力时间
                player.crouchTime = 0;

                player.currentState.RemoveState(this);
                
            }
            if (player.latch.Idle > 0)
            {
                player.currentState.AddState(new IdleState(player));
            }

        }

        public override void Enter()
        {
            timer = 0;
            player.latch.Jump--;
            player.rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

        }
        public override void Exit()
        {
            player.latch.jumpInit();
        }



    }
}