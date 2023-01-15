using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInteract : MonoBehaviour
{

    // Start is called before the first frame update
    private Rigidbody2D rb;
    public Animator anim;
    HealthBar healthBar;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        checkRadius = FallCheck.GetComponent<CircleCollider2D>().radius;
      

    }

    // Update is called once per frame
    void Update()
    {
        Fail();
       

    }
    private void FixedUpdate()
    {
        CheckingFall();
    }
    public bool onFall;
    public Transform FallCheck;
    private float checkRadius = 0.12f;
    public LayerMask PlaceDeath;

    public Transform respawn;
    void Fail()
    {
        if (onFall)
        {

            transform.position = new Vector3(respawn.position.x, respawn.position.y, respawn.position.z);
            PlayerControl.blockMoveXYforLedge = true;
            PlayerControl.jumpLock = true;
            rb.gravityScale = 1;
        }
    }
    void CheckingFall()
    {
        onFall = Physics2D.OverlapCircle(FallCheck.position, checkRadius, PlaceDeath);
        anim.SetBool("Death", onFall);
    }
    void FinishRespawn()
    {
        PlayerControl.blockMoveXYforLedge = false;
        PlayerControl.jumpLock = false;
        
        
    }

    
}
