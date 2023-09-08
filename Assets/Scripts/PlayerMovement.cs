using Assets.Scripts.PlayerState;
using Assets.Scripts.PlayerState.PlayerCompositeState;
using Assets.Scripts.PlayerState.PlayerState;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum GroundObjetct{
        GROUND,
        NULL
    }
    
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;

    [Header("所以人物状态的状态锁")]
    public StatusLatch latch;

    [Header("状态")]
    //private _PlayerState currentState;
    public _PlayerCompositeState currentState;

    [Header("移动参数")]
    public float speed;

    [Header("下蹲时移动速度的乘值")]
    public float crouchSpeed = 0.3f;

    [Header("玩家在标准状态下的速度")]
    public float standardSpeed = 8f;

    [Header("人物底部接触的物体")]
    public GroundObjetct isTouch;
    //public float crouchSpeedDivisor = 3f;

    [Header("下蹲时人物碰撞体y轴长度和位置的乘数")]
    public float crouchY = 0.5f;

    [Header("人物下蹲时的蓄力时间")]
    public float crouchTime = 0;

    [Header("人物第一次跳跃的标记时刻")]
    private float jumpSignTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        isTouch = GroundObjetct.NULL;
        latch = new StatusLatch();
        latch.Init();
        speed = standardSpeed;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentState = new _PlayerCompositeState(this);
        currentState.AddState(new IdleState(this));
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("有在运行？");
        currentState.Update();
//        Debug.Log(currentState._ToString());
    }


    private void FixedUpdate()
    {
        
    }


    public void SetStates(_PlayerCompositeState states)
    {
        currentState.Exit();
        currentState = states;
        currentState.Enter();
    }

    public void ChangeState(_PlayerState state)
    {
        currentState.Exit();
        //currentState = state;
        currentState.Enter();
    }

    /// <summary>
    /// 跳跃的判断逻辑和执行逻辑，不删除状态
    /// </summary>
    /// <returns>返回是否执行了跳跃的逻辑</returns>
    public bool isJump()
    {
        if (Input.GetButtonDown("Jump")) {

            switch (this.latch.Jump)
            {
                case 0:
                    return false;
                case 1:
                    float intervalTime = Time.time - jumpSignTime;
                    if(intervalTime > 0.2)
                    {
                        this.currentState.AddState(new JumpState(this));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case 2:
                    jumpSignTime = Time.time;

                    //这个判断分支为判断是否是蹲下蓄力跳跃
                    if (currentState.searchState(typeof(CrouchState)))
                    {
                        currentState.AddState(new JumpState(this, true));
                      //  currentState.RemoveState(currentState.GetState(typeof(CrouchState)));
                    }
                    else
                    {
                        this.currentState.AddState(new JumpState(this));
                    }
                    
                    return true;
            }


        }
        return false;
        
    }
    /// <summary>
    /// 跳跃的判断逻辑和执行逻辑，可删除一个状态
    /// </summary>
    /// <param name="removeState">需要删除的状态</param>
    /// <param name="addState">需要增加的状态</param>
    /// <returns>返回是否执行了跳跃的逻辑</returns>
    public bool isJump(_PlayerState removeState, _PlayerState addState)
    {
        if (isJump())
        {
            if (removeState != null) { currentState.RemoveState(removeState); }
            if (addState != null) { currentState.AddState(addState); }
            return true;
        }
        return false;
        
    }


    public bool isCrouch()
    {
        if (Input.GetButton("Crouch") && latch.Crouch > 0 && !currentState.searchState(typeof(JumpState)))
        {
            currentState.AddState(new CrouchState(this));
            return true;
        }

        return false;
    }

    public bool isCrouch(_PlayerState removeState, _PlayerState addState)
    {
        if (isCrouch())
        {
            if(removeState != null) { currentState.RemoveState(removeState); }
            if(addState != null) { currentState.AddState(addState); }
            return true;
        }
        return false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision == null)
        {
            isTouch = GroundObjetct.NULL;
        }else if(collision.gameObject.tag == "Ground")
        {
            isTouch = GroundObjetct.GROUND;
            
        }

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouch = GroundObjetct.NULL;
    }


}
