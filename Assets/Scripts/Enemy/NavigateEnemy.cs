using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NavigateEnemy : MonoBehaviour
{
    [Header("Main Param")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D colider;
    [SerializeField] private IK ik;
    [Header("Checkers")]
    [Header("Ground")]
    [SerializeField] private bool onGround;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float checkRadius = 0.12f;
    [SerializeField] private LayerMask Ground;
    [Header("Wall")]
    [SerializeField] private bool onWall;
    [SerializeField] private bool onWallUp;
    [SerializeField] private bool onWallDown;
    [SerializeField] Transform WallCheckUp;
    [SerializeField] Transform WallCheckDown;
    [SerializeField] private LayerMask Wall;
    [Header("Ledge")]
    [SerializeField] private float WallRayDistance;
    [SerializeField] public bool onLedge;
    [SerializeField] public bool onSmallLedge;
    [SerializeField] public bool onHighLedge;
    [SerializeField] private float LedgeYRayDistance;
    [SerializeField] private float LedgeHeight;
    [SerializeField] private float LedgeYDownDistance;
    [Header("Cliff")]
    [SerializeField] private float CliffDepth;
    [SerializeField] public bool onSmallCliff;
    [SerializeField] public bool onHighCliff;
    [SerializeField] public bool onCliff;
    [SerializeField] private float fallingDistance;
    [SerializeField] private float fallingSpeed;

    [Header("AnimOffset")]
    [SerializeField] Transform SmallLedge;
    [SerializeField] public Transform ToStandUp;
    [SerializeField] Transform HighCliff;
    [SerializeField] Transform FinishHighCliff;
    [SerializeField] float SmallLedgeDistance;

    [SerializeField] public bool stopMove;
    [SerializeField] bool startSubState;
    [Header("TestParam")]
    [SerializeField] bool WalkTest;
    [SerializeField] bool RightWalk;
    [SerializeField] int Walkright;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ik=GetComponent<IK>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMove)
        {
            if (WalkTest)
                Testwalk();
            if (RightWalk)
                Walkright = -1;
            else
                Walkright = 1;
        }
        CalcfallingSpeed();
    }
    private void FixedUpdate()
    {
        WallCheck();
        LedgeCheck();
        CliffCheck();
        CheckingGround();

    }
    



    void WallCheck()
    {
        onWallUp = Physics2D.Raycast(WallCheckUp.position, new Vector2(transform.localScale.x * Enemy.fliping, 0f), WallRayDistance, Wall);
        onWallDown = Physics2D.Raycast(WallCheckDown.position, new Vector2(transform.localScale.x * Enemy.fliping, 0f), WallRayDistance, Wall);
        if(onWallUp && onWallDown)
        {
            onWall = true;
        }
        else onWall = false;
    }
    void LedgeCheck()
    {
        LedgeHeight= Physics2D.Raycast(new Vector2(WallCheckUp.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckUp.position.y + LedgeYRayDistance), Vector2.down, LedgeYDownDistance, Wall).distance;

        if (LedgeHeight > 1.4f && LedgeHeight < 2.4f&&!onHighLedge&& !onCliff)
        {
            onSmallLedge = true;
            onHighLedge = false;
            //Small ledge
            AnimStart();
            onLedge = true;
            anim.SetTrigger("Small Ledge");
            offsetAnimSmallLedge();


        }
        else if (LedgeHeight > 0 && LedgeHeight < 1.4f&& !onSmallLedge&& !onCliff)
        {
            onSmallLedge = false;
            onHighLedge = true;
            // IK Anim JumpLedge
            AnimStart();
            onLedge = true;
            anim.SetTrigger("High Ledge");
            
            
        }
        anim.SetBool("HighLedge", onHighLedge);
        anim.SetBool("SmallLedge", onSmallLedge);


    }
    void CliffCheck()
    {
        CliffDepth= Physics2D.Raycast(new Vector2(WallCheckDown.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckDown.position.y), Vector2.down, 3f, Wall).distance;

        if(CliffDepth >1.5f&& CliffDepth < 2.5f&&!onLedge&& !onSmallCliff&& onGround)
        {
            onCliff = true;
            AnimStart();
            onSmallCliff = false;
            onHighCliff = true;
            anim.SetTrigger("HighDepth");

            offsetHighJump();
        }
        else if(CliffDepth <1.5f&& CliffDepth > 0.5f&&!onLedge&&!onHighCliff&& onGround)
        {
            onCliff = true;
            AnimStart();
            onSmallCliff = true;
            onHighCliff = false;
            anim.SetTrigger("Depth");

            offsetHighJump();
        }

        anim.SetBool("DepthBool", onSmallCliff);
        anim.SetBool("HighDepthBool", onHighCliff);
        
    }
    void subState()
    {
        startSubState= !startSubState;
        anim.SetBool("SubState", startSubState);
    }
    void tpClimb()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 1.572f);
    }
    void Testwalk()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.right, 1f* Time.deltaTime * Walkright);
    }
  
  
    // Animation calc
    void FinishSmallledgePosition()
    {
        transform.position = SmallLedge.position;
        AnimFinish();
        
    }
    void FinishJumpDownPosition()
    {
        transform.position = HighCliff.position;
        AnimFinish();
        
    }
    void offsetHighJump()
    {
        if (onHighCliff)
        {
            fallingDistance = CliffDepth - 1.3f;
            fallingSpeed = fallingDistance / 0.66f;
        }
        if (onSmallCliff)
        {
            fallingDistance = CliffDepth - 1.3f;
            fallingSpeed = fallingDistance / 0.66f;
        }
        transform.position = new Vector2(HighCliff.position.x, HighCliff.position.y);
    }
    void CalcfallingSpeed()
    {
        //default speed 1.965 m/s
        //time 660ms
        //default distance 1.297 m
        //if distance 2.7 m
        //time 660ms
        //calc distance 2.7m-1.297m-0.34m = 1.063m
        // calc speed 1.403m / 660ms =2.1m/s
        //offset distance 0.34m
        if (mover)
        {
            
            transform.Translate(0f, fallingSpeed*Time.deltaTime*-1, 0f);
        }
    }
    void FinishHighPosition()
    {
        transform.position = new Vector2(FinishHighCliff.position.x, FinishHighCliff.position.y);
    }
    bool mover;
    void moverOn()
    {
        mover = true;
    }
    void moverOff()
    {
        mover = false;
    }
    void AnimStart()
    {

        stopMove= true;
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0f;
        colider.enabled = false;
        
    }
    void AnimFinish()
    {
        colider.enabled = true;
        rb.gravityScale = 1f;
        stopMove= false;
    }
    void offsetAnimSmallLedge()
    {
        SmallLedgeDistance = Physics2D.Raycast(SmallLedge.position, Vector2.down, 1.7f, Wall).distance;
        transform.position = new Vector2(transform.position.x, transform.position.y - SmallLedgeDistance);
    }
    
    private void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }




    // Animation calc


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(WallCheckUp.position, new Vector2(WallCheckUp.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckUp.position.y));
        Gizmos.DrawLine(WallCheckDown.position, new Vector2(WallCheckDown.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckDown.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(WallCheckUp.position.x + WallRayDistance, WallCheckUp.position.y + LedgeYRayDistance), 
            new Vector2(WallCheckUp.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckUp.position.y + LedgeYRayDistance - LedgeHeight));
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector2(WallCheckDown.position.x + WallRayDistance, WallCheckDown.position.y), 
            new Vector2(WallCheckDown.position.x + WallRayDistance * transform.localScale.x * Enemy.fliping, WallCheckDown.position.y- CliffDepth));
    }
}
