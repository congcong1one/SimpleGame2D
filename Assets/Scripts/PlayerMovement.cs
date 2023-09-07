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

    [Header("��������״̬��״̬��")]
    public StatusLatch latch;

    [Header("״̬")]
    //private _PlayerState currentState;
    public _PlayerCompositeState currentState;

    [Header("�ƶ�����")]
    public float speed;

    [Header("�¶�ʱ�ƶ��ٶȵĳ�ֵ")]
    public float crouchSpeed = 0.3f;

    [Header("����ڱ�׼״̬�µ��ٶ�")]
    public float standardSpeed = 8f;

    [Header("����ײ��Ӵ�������")]
    public GroundObjetct isTouch;
    //public float crouchSpeedDivisor = 3f;

    [Header("�¶�ʱ������ײ��y�᳤�Ⱥ�λ�õĳ���")]
    public float crouchY = 0.5f;

    [Header("�����¶�ʱ������ʱ��")]
    public float crouchTime = 0;

    [Header("�����һ����Ծ�ı��ʱ��")]
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
        Debug.Log("�������У�");
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
    /// ��Ծ���ж��߼���ִ���߼�����ɾ��״̬
    /// </summary>
    /// <returns>�����Ƿ�ִ������Ծ���߼�</returns>
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

                    //����жϷ�֧Ϊ�ж��Ƿ��Ƕ���������Ծ
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
    /// ��Ծ���ж��߼���ִ���߼�����ɾ��һ��״̬
    /// </summary>
    /// <param name="removeState">��Ҫɾ����״̬</param>
    /// <param name="addState">��Ҫ���ӵ�״̬</param>
    /// <returns>�����Ƿ�ִ������Ծ���߼�</returns>
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
