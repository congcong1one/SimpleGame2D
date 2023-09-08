using Assets.Scripts.PlayerState.PlayerState;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.PlayerState
{
    public class MoveState : _PlayerState
    {

        float horizontal;
        bool isJump;

        public MoveState(PlayerMovement player) : base(player) { }

        public override void Enter()
        {
            player.latch.Move--;
            horizontal = 0;
        }
        public override void Exit()
        {
            player.latch.Move++;
        }
        public override void Update() 
        {
            float temp = Input.GetAxis("Horizontal");

            //如果松开移动按键，角色停止移动
            if(Mathf.Abs(temp) - Mathf.Abs(horizontal) < 0 || temp == 0 )
            {
                player.currentState.RemoveState(this);
                return;
            }

            //转换到跳跃状态
            if (player.isJump())
            {
                return;
            }

            //转换到下蹲状态
            if(player.isCrouch())
            {
                return;
            }

            

            //角色移动
            horizontal = temp;
            GroundMovement(horizontal);
            

            
        }

        /// <summary>
        /// 在地面上的移动逻辑
        /// </summary>
        /// <param name="xVelocity">移动的速度（一般为horizontal的值）</param>
        public void GroundMovement(float xVelocity)
        {


            
           player.rb.velocity = new Vector2(xVelocity * player.speed, player.rb.velocity.y);

            filpDirction(xVelocity);
        }

        /// <summary>
        /// 调整角色的面向方位
        /// </summary>
        /// <param name="xVelocity">移动的速度（一般为horizontal的值）</param>
        public void filpDirction(float xVelocity)
        {
            if (xVelocity > 0)
            {
                player.transform.localScale = new Vector2(1, 1);
            }
            if (xVelocity < 0)
            {
                player.transform.localScale = new Vector2(-1, 1);
            }
        }

    }
}