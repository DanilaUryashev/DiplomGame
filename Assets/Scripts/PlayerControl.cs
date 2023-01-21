using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private bool faceRight = true;
    public Animator anim;
    void Start()
    {
        swordHand.SetActive(false);
        swordSpine.SetActive(true);
        attackPoint.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checkRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        topCheckRadius = TopCheck.GetComponent<CircleCollider2D>().radius;
        realSpeed = turnSpeed;
        WallCheckRadiusDown = WallCheckDown.GetComponent<CircleCollider2D>().radius;
    }
    public bool asd;
    // Update is called once per frame
    void Update()
    {
        asd = jumpLock;
        walk();
        run();
        crouch();
        if (Time.time >= nextTimeEqp)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                EquipSword();
                nextTimeEqp = Time.time + 1f / EqpRate;
            }
        }
        MoveY = rb.velocity.y;
        if (MoveY < 0)
        {
            anim.SetBool("fail", true);
        }
        else
        {
            anim.SetBool("fail", false);

        }
    }
    private void FixedUpdate()
    {
        CheckingWall();
        CheckingLedge();
        CheckingGround();
        CheckingRoof();
        Checkflip();
    }
    public float MoveY;
    public bool jumping = false;

    void jump()
    {
        if (onGround && !jumpLock)
        { 
            rb.AddForce(Vector2.up * jumpForce);
        }       
    }



    public static float horizontal;
    public float turnSpeed = 3;
    public float fastSpeed = 5;
    public float realSpeed;
    public float MoveX;
    public bool speedLock;
    void walk()
    {
        
        if (!blockMoveXYforLedge) 
        { 
                horizontal = Input.GetAxisRaw("Horizontal") * realSpeed * Time.deltaTime;
                MoveX = Mathf.Abs(horizontal);
                anim.SetFloat("MoveX", MoveX);
                transform.Translate(0, 0, horizontal);
                if (horizontal > 0 && !faceRight)
                {
                    flip();
                }
                else if (horizontal < 0 && faceRight)
                {
                    flip();
                }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
       


    }
    public Transform TopCheck;
    private float topCheckRadius=0.03f;
    public LayerMask Roof; 
    public Collider2D poseStand;
    public Collider2D poseSquat;

    public static bool jumpLock= false;
    private bool crouching;
    void crouch()
    {
        if (Input.GetKey(KeyCode.C)&& !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("crouch", true);
            poseStand.enabled = false;
            poseSquat.enabled = true;
            jumpLock = true;
            crouching = true;


        }
        else if (!onRoof) 
        { 
            anim.SetBool("crouch", false);
            poseStand.enabled = true;
            poseSquat.enabled = false;
            jumpLock = false;
            crouching = false;
        }
    }
    void run()
    {


        if (Input.GetKey(KeyCode.LeftShift) && !crouching)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.SetBool("run", true);
                if (MoveY == 0)
                    realSpeed = fastSpeed;
                if (Input.GetKey(KeyCode.Space))
                { }
                
            }
        }
           
        else
        {
            anim.SetBool("run", false); ;

            if (!speedLock) realSpeed = turnSpeed;
            else if (speedLock && onGround) speedLock = false;
            else realSpeed = fastSpeed;
            anim.SetBool("run", false);
        }
        runningSlide();

    }
   
    public float jumpForce = 350f;
   
    void flip()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z * -1);
        
        
    }
    private float fliping = 1;
    void Checkflip()
    {
        if (faceRight)
        {
            fliping = 1;
        }
        else
        {
            fliping = -1;
        }
    }

    public bool onGround;
    public bool onRoof;
    public Transform GroundCheck;
    private float checkRadius = 0.12f;
    public LayerMask Ground;

    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }
    void CheckingRoof()
    {
        onRoof = Physics2D.OverlapCircle(TopCheck.position, topCheckRadius, Roof);
        anim.SetBool("onRoof", onGround);
    }
    public bool onWall;
    public bool onWallUp;
    public bool onWallDown;
    public LayerMask Wall;
    public Transform WallCheckUp;
    public Transform WallCheckDown;
    public float WallCheckRayDistance = 1f;
    private float WallCheckRadiusDown;
    public bool onLedge;
    public float ledgeRayCorrectY = 0.5f;
    void CheckingWall()
    {
        onWallUp = Physics2D.Raycast
        (
        WallCheckUp.position,
        new Vector2(transform.localScale.x * fliping, 0),
        WallCheckRayDistance,
        Wall
        );
        onWallDown = Physics2D.OverlapCircle(WallCheckDown.position, WallCheckRadiusDown, Wall);
        onWall = (onWallUp && onWallDown);
        anim.SetBool("onWall", onWall);
      
    }
    void CheckingLedge()
    {
        if (onWallUp)
        {
            onLedge = !Physics2D.Raycast
            (
            new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY),
            new Vector2(transform.localScale.x * fliping, 0),
            WallCheckRayDistance,
            Wall
            );
        }
        else { onLedge = false; }
        anim.SetBool("onLedge", onLedge);
        
        if ((onLedge && Input.GetAxisRaw("Vertical") != -1) || blockMoveXYforLedge)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            offsetCalculateAndCorrect();
        }
    }
    public float slideSpeed;
    void runningSlide()
    {
        if (Input.GetKey(KeyCode.C))
        {
            jumpLock = true;
            if (Input.GetKeyDown(KeyCode.C))
            {

                speedLock = true;
                anim.SetTrigger("runnigSlide");
                slideSpeed = horizontal;
                realSpeed -= 4;
            }
        }
        
        
    }
    void FinishSlide()
    {
        jumpLock = false;
        blockMoveXYforLedge = false;
        rb.gravityScale = 1;
    }

    public float minCorrectDistance = 0.01f;
    public float offsetY;
    void offsetCalculateAndCorrect()
    {
        offsetY = Physics2D.Raycast
        (
        new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x * fliping,
        WallCheckUp.position.y + ledgeRayCorrectY),
        Vector2.down,
        ledgeRayCorrectY,
        Ground
        ).distance;
        if (offsetY > minCorrectDistance * 1.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - offsetY +
            minCorrectDistance, transform.position.z);
            blockMoveXYforLedge = true;
            jumpLock = true;

        }
    }
    public static bool blockMoveXYforLedge;

    public Transform finishLedgePosition;
    void FinishLedge()
    {
        transform.position = new Vector3(finishLedgePosition.position.x, finishLedgePosition.position.y,
        finishLedgePosition.position.z);
        anim.Play("idle");
        blockMoveXYforLedge = false;
        rb.gravityScale = 1;
       
    }
    public bool eqpSwordStatus=false;
    private int eqpSword = 0;

    public float EqpRate = 2f;
    public float nextTimeEqp = 0f;
    void EquipSword()
    {
        eqpSword += 1;

        if (eqpSword == 1 && eqpSwordStatus == false)
        {
            eqpSwordStatus = true;
            anim.SetBool("EquipSword", eqpSwordStatus);
            anim.SetTrigger("EqpSwordTriger");
        }
        else if ( eqpSword == 2 && eqpSwordStatus == true)
        {
            eqpSwordStatus = false;
            anim.SetBool("EquipSword", eqpSwordStatus);
            anim.SetTrigger("EqpSwordTriger");
            eqpSword = 0;
        }
       

    }
    
    void eqpSwordAnimationController()
    {
        if (eqpSwordStatus)
        {
            swordSpine.SetActive(false);
            swordHand.SetActive(true);
            attackPoint.SetActive(true);
        }
        else
        {
            attackPoint.SetActive(false);
            swordSpine.SetActive(true);
            swordHand.SetActive(false);
        }
       
    }


    public GameObject swordSpine;
    public GameObject attackPoint;
    public GameObject swordHand;

    private void OnDrawGizmos()
    {


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(WallCheckUp.position, new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x* fliping, WallCheckUp.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawLine
        (
        new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY),
        new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x* fliping,
        WallCheckUp.position.y + ledgeRayCorrectY)
        );

        Gizmos.color = Color.green;
        Gizmos.DrawLine
        (
            new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x* fliping,
            WallCheckUp.position.y + ledgeRayCorrectY),
            new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x* fliping,
            WallCheckUp.position.y)
        );
    }

}
