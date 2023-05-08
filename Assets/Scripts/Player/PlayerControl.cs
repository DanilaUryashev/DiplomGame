using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    public PlayerInput playerInput;

    private Rigidbody2D rb;

    public bool faceRight = true;

    public Animator anim;

    [Header("Resolution Setting"), SerializeField]
    private TMP_Text GameText;

    [SerializeField]
    private TMP_Text GraphicText;

    [SerializeField]
    private TMP_Text ControlText;

    [SerializeField]
    private GameObject GameSetting;

    [SerializeField]
    private GameObject GraphSetting;

    [SerializeField]
    private GameObject ControlSetting;

    [SerializeField]
    private GameObject Menu;

    public bool asd;

    public float MoveY;

    public bool jumping;

    public static float horizontal;

    public float turnSpeed = 3f;

    public float fastSpeed = 5f;

    public float slideSpeed = 6f;

    public float realSpeed;

    public float MoveX;

    public bool speedLock;

    public Transform TopCheck;

    private float topCheckRadius = 0.03f;

    public LayerMask Roof;

    public Collider2D poseStand;

    public Collider2D poseSquat;

    public static bool jumpLock = false;

    public bool crouching;

    public string crouchHey;

    public bool runing;

    public float jumpForce = 350f;

    public Vector3 xAxisDirection = new Vector3(0f, 90f, 0f);

    public static float fliping = 1f;

    public bool onGround;

    public bool onRoof;

    public Transform GroundCheck;

    private float checkRadius = 0.12f;

    public LayerMask Ground;

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

    private bool onSlide;

    public float SlidingForce = 4f;

    public Collider2D SlidingPose;

    private bool climbing;

    public float minCorrectDistance = 0.01f;

    public float offsetY;

    public static bool blockMoveXYforLedge;

    public Transform finishLedgePosition;

    public static bool eqpSwordStatus = false;

    private int eqpSword;

    public float EqpRate = 2f;

    public float nextTimeEqp;

    public GameObject swordSpine;

    public GameObject attackPoint;

    public GameObject swordHand;

    private void Start()
    {
        swordHand.SetActive(false);
        swordSpine.SetActive(true);
        attackPoint.SetActive(false);
        rb = base.GetComponent<Rigidbody2D>();
        anim = base.GetComponent<Animator>();
        checkRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        topCheckRadius = TopCheck.GetComponent<CircleCollider2D>().radius;
        realSpeed = turnSpeed;
        WallCheckRadiusDown = WallCheckDown.GetComponent<CircleCollider2D>().radius;
    }

    public void openMenu()
    {
        GameText.color = Color.red;
        GraphicText.color = Color.white;
        ControlText.color = Color.white;
        GameSetting.SetActive(true);
        GraphSetting.SetActive(false);
        ControlSetting.SetActive(false);
        if (!Menu.activeInHierarchy)
        {
            Menu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Menu.SetActive(false);
            Time.timeScale = 1f;
        }
        Start();
    }

    public void Update()
    {
        walk();
        MoveY = rb.velocity.y;
        if (MoveY < 0f)
        {
            anim.SetBool("fail", true);
        }
        anim.SetBool("fail", false);
    }

    private void FixedUpdate()
    {
        CheckingWall();
        CheckingLedge();
        CheckingGround();
        CheckingRoof();
        Checkflip();
    }

    public void jump(bool jumpKey)
    {
        if ((onGround && !jumpLock && !onSlide) & jumpKey)
        {
           rb.AddForce(Vector2.up * jumpForce);
            jumping = true;
        }
    }

    public void walk()
    {
        if (!blockMoveXYforLedge && !onSlide)
        {
            horizontal = Input.GetAxisRaw("Horizontal") * realSpeed * Time.deltaTime*fliping;
            MoveX = horizontal;
            anim.SetFloat("MoveX", MoveX);
            transform.Translate(0f, 0f, horizontal);
            if (horizontal < 0f)
            {
                flip();
            }


        }
        if (!runing)
        {
            realSpeed = turnSpeed;
        }
    }

    public void crouch(bool crouchkey)
    {
        if (crouchkey)
        {
            anim.SetBool("crouch", true);
            poseStand.enabled = false;
            poseSquat.enabled = true;
            jumpLock = true;
            crouching = true;
            if (runing)
            {
                runningSlide();
            }
        }
        else if (!onRoof && !crouchkey)
        {
            anim.SetBool("crouch", false);
            poseStand.enabled = true;
            poseSquat.enabled = false;
            jumpLock = false;
            crouching = false;
        }
    }

    public void run(bool runKey)
    {
        if (runKey && !crouching)
        {
            anim.SetBool("run", true);
            realSpeed = fastSpeed;
            runing = true;
            return;
        }
        anim.SetBool("run", false);
        runing = false;
        if (!speedLock)
        {
            realSpeed = turnSpeed;
        }
        else if (speedLock && onGround)
        {
            speedLock = false;
        }
        else
        {
            realSpeed = fastSpeed;
        }
        anim.SetBool("run", false);
    }

    public void flip()
    {
        Debug.Log("123");
        faceRight = !faceRight;
        //transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z *-1);
        transform.forward *=-1 ;
    }

    private void Checkflip()
    {
        if (faceRight)
        {
            fliping = 1f;
        }
        else
        fliping = -1f;
    }

    private void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, Ground);
        anim.SetBool("onGround", onGround);
    }

    private void CheckingRoof()
    {
        onRoof = Physics2D.OverlapCircle(TopCheck.position, topCheckRadius, Roof);
        anim.SetBool("onRoof", onGround);
    }

    private void CheckingWall()
    {
        onWallUp = Physics2D.Raycast(WallCheckUp.position, new Vector2(base.transform.localScale.x * fliping, 0f), WallCheckRayDistance, Wall);
        onWallDown = Physics2D.OverlapCircle(WallCheckDown.position, WallCheckRadiusDown, Wall);
        onWall = (onWallUp && onWallDown);
        anim.SetBool("onWall", onWall);
    }

    private void CheckingLedge()
    {
        if (onWallUp)
        {
            onLedge = !Physics2D.Raycast(new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY), new Vector2(base.transform.localScale.x * fliping, 0f), WallCheckRayDistance, Wall);
        }
        else
        {
            onLedge = false;
        }
        anim.SetBool("onLedge", onLedge);
        if (onLedge && (Input.GetKey(KeyCode.Space) || MoveY < 0f))
        {
            offsetCalculateAndCorrect();
            rb.gravityScale = 0f;
            blockMoveXYforLedge = true;
            poseStand.enabled = false;
            poseSquat.enabled = false;
            SlidingPose.enabled = false;
            rb.velocity = new Vector2(0f, 0f);
            anim.SetTrigger("Climbing");
        }
    }

    private void offsetCalculateAndCorrect()
    {
        offsetY = Physics2D.Raycast(new Vector2(WallCheckUp.position.x + WallCheckRayDistance * transform.localScale.x * fliping, WallCheckUp.position.y + ledgeRayCorrectY), Vector2.down, ledgeRayCorrectY, Ground).distance;
        if (offsetY < minCorrectDistance)
        {
            transform.position = new Vector3(transform.position.x + 0.3183f, transform.position.y - offsetY, transform.position.z);
            blockMoveXYforLedge = true;
            jumpLock = true;
        }
    }

    private void FinishLedge()
    {
        transform.position = new Vector3(finishLedgePosition.position.x, finishLedgePosition.position.y, finishLedgePosition.position.z);
        anim.Play("idle");
        blockMoveXYforLedge = false;
        rb.gravityScale = 1f;
        climbing = false;
        poseStand.enabled = true;
        jumpLock = false;
    }

    private void runningSlide()
    {
        poseStand.enabled = false;
        poseSquat.enabled = false;
        SlidingPose.enabled = true;
        onSlide = true;
        speedLock = true;
        anim.SetTrigger("runnigSlide");
        rb.AddForce(Vector2.right * SlidingForce * fliping * 100f);
    }

    private void FinishSlide()
    {
        SlidingPose.enabled = false;
        speedLock = false;
        onSlide = false;
        jumpLock = false;
        blockMoveXYforLedge = false;
        rb.gravityScale = 1f;
    }

    public void EquipSword()
    {
        eqpSword++;
        if (eqpSword == 1 && !eqpSwordStatus)
        {
            eqpSwordStatus = true;
            anim.SetBool("EquipSword", eqpSwordStatus);
            anim.SetTrigger("EqpSwordTriger"); 
        }
        if (eqpSword == 2 && eqpSwordStatus)
        {
            eqpSwordStatus = false;
            anim.SetBool("EquipSword", eqpSwordStatus);
            anim.SetTrigger("EqpSwordTriger");
            eqpSword = 0;
        }
    }

    private void eqpSwordAnimationController()
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(WallCheckUp.position, new Vector2(WallCheckUp.position.x + WallCheckRayDistance * base.transform.localScale.x * fliping, WallCheckUp.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(WallCheckUp.position.x, WallCheckUp.position.y + ledgeRayCorrectY), new Vector2(WallCheckUp.position.x + WallCheckRayDistance * base.transform.localScale.x * fliping, WallCheckUp.position.y + ledgeRayCorrectY));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(WallCheckUp.position.x + WallCheckRayDistance * base.transform.localScale.x * fliping, WallCheckUp.position.y + ledgeRayCorrectY), new Vector2(WallCheckUp.position.x + WallCheckRayDistance * base.transform.localScale.x * fliping, WallCheckUp.position.y));
    }
}
