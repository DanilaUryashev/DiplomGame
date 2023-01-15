using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float patrolPosition;
    public Transform patrolPoint;
    bool moveingRight;

    Transform player;
    
    public float stoppingDistance;

    bool chill = false;
    bool angry = false;
    bool goBack = false;

    private bool faceRight = true;
  

    public Animator anim;

    public float MaxHealth = 100f;
    public float currentHealth;



    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = MaxHealth;  
    }
    private void Update()
    {
        
        if (Vector2.Distance(transform.position, patrolPoint.position) < patrolPosition&& angry==false)
        {
            chill = true;
        }
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false;
        }
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            goBack = true;
            angry = false;
        }
        if (chill == true)
        {
            Chill();
        }
        else if(angry == true)
        {
            Angry();
        }
        else if (goBack == true)
        {
            Goback();
        }
        Attack();
    }
    private void FixedUpdate()
    {
        Checkflip();
        CheckDistanceAtk();
        DamagePlayer();
    }

    public float walk;
    
    void Chill()
    {
        anim.SetBool("Walk", true);
        if (transform.position.x> patrolPoint.position.x + stoppingDistance)
        {
            moveingRight = false;
        }
        else if(transform.position.x<patrolPoint.position.x - stoppingDistance)
        {
            moveingRight = true;
        }
        if (moveingRight)
        {
            
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            if (!faceRight)
            {
                flip();
            }

        }
        else
        {
            
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            if (faceRight)
            {
                flip();
            }
        }
         
    }
    bool eqpWeapon=false;
    void FinishEqpWeapon()
    {
        eqpWeapon = true;
        anim.SetBool("EqpWeapon", true);
        Angry();
    }

    void Angry()
    {
        anim.SetBool("Walk", false);
        if (eqpWeapon)
        {
           
            anim.SetBool("Angry", true);
            speed = 2f;
            if (Atk == false&&AtkFinish==true) { 
                if (moveingRight)
                {

                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    if (!faceRight)
                    {
                        flip();
                    }

                }
                else
                {

                    transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    if (faceRight)
                    {
                        flip();
                    }
                }
            }
            
            
        }
        else
        {
            anim.SetBool("Angry", true);
        }
        if (transform.position.x > player.position.x)
        {
            moveingRight = false;
        }
        else if (transform.position.x < player.position.x)
        {
            moveingRight = true;
        }

    }
    void Goback()
    {
        anim.SetBool("Angry", false);
        anim.SetBool("Walk", true);
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint.position, speed * Time.deltaTime);
    }
    void flip()
    {
        faceRight = !faceRight;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
        

    }
    public bool onPlayer;
    public Transform PlayerCheck;
    public float PlayerCheckRayDistance = 1f;
    public LayerMask Player;
    void CheckDistanceAtk()
    {
        onPlayer = Physics2D.Raycast
        (
        PlayerCheck.position,
        new Vector2(transform.localScale.x*fliping, 0),
        PlayerCheckRayDistance,
        Player
        );
        anim.SetBool("Attack", onPlayer);
        

    }
    private float fliping = 1;
    void Checkflip()
    {
        if (moveingRight)
        {
            fliping = 1;
        }
        else
        {
            fliping = -1;
        }
    }
    void PlayerDetected()
    {
        if (eqpWeapon)
        {
            Angry();
        }
        else
        {
            eqpWeapon = true;
            Angry();
        }
        
    }
    private bool Atk;
    private bool AtkFinish=true;
    void FinishAttack()
    {
        AtkFinish = true;
        AtkCombo += 0.5f;
        anim.SetFloat("Blend", AtkCombo);

    }
    public float AtkCombo;
    void Attack()
    {
        
        if (onPlayer)
        {
            Atk = true;
            AtkFinish = false;

            anim.SetBool("Attack", Atk);

        }
        else
        {
            AtkCombo = 0f;
            Atk = false;
        }
       

    }
    public bool playerHit=false;
    void PlayerHit()
    {
        if (onPlayer)
        {
            playerHit = true;
        }
    }
    public float damagePlayer=15f;
    void DamagePlayer()
    {
        if (playerHit)
        {
            HealthBar.playerHealth-= damagePlayer;
            playerHit = false;
        }
        else playerHit = false;
    }

    public void TakeDamage(float damage)
    {

        anim.SetTrigger("TakeDamage");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            
            Die();
        }
    }

    public void Die()
    {
        anim.SetBool("IsDead", true);
        GetComponentInChildren<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        this.enabled = false;
    }



    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(PlayerCheck.position, new Vector2(PlayerCheck.position.x + PlayerCheckRayDistance* transform.localScale.x*fliping, PlayerCheck.position.y));
    }

}
