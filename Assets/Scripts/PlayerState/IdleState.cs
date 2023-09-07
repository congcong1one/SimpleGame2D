using System.Collections;
using UnityEngine;
namespace Assets.Scripts.PlayerState.PlayerState
{
    public class IdleState : _PlayerState
    {
        
        public IdleState(PlayerMovement player) : base(player) { }

        public override void Enter()
        {
            player.latch.Idle--;
        }
        public override void Update()
        {


            //转换到跳跃状态
            if (player.isJump())
            {
                return;
            }

            //转换到移动状态
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 && player.latch.Move > 0)
            {

                player.currentState.AddState(new MoveState(player));
                
                //player.ChangeState(new MoveState(player));
                return;
            }

            //转换到下蹲状态
            if(player.isCrouch())
            {
                return;
            }
        }
        public override void Exit()
        {
            player.latch.Idle++;
        }


    }
}