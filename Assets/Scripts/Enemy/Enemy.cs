using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float patrolPosition;
    public Transform patrolPoint;
    bool moveingRight;
    [Header("MainParam")]
    [SerializeField] private NavigateEnemy navEnemy;
    public Animator anim;
    [SerializeField] private Transform player;
    
    [Header("12312321")]
    [SerializeField] public static float fliping = 1;
    public float stoppingDistance;
    [Header("State")]
    [Header("Patrol State")]
    [SerializeField] private bool chill = false;
    [SerializeField] private bool angry = false;
    [SerializeField] private bool goBack = false;
    [Header("Anxiety State")]
    [SerializeField] private bool Attention;
    [SerializeField] private float AttentionTime;

    private bool faceRight = true;
  

    
    [Header("HP")]
    [SerializeField] public float MaxHealth = 100f;
    [SerializeField] public float currentHealth;

    [Header("FOV")]
    [SerializeField] public Transform pfFieldOfView;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 10f;

    private FieldOfView fieldOfView;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navEnemy = GetComponent<NavigateEnemy>();
        currentHealth = MaxHealth;

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
    }
    private void Update()
    {

        StatePatrol();
        fieldOfView.SetOrigin(new Vector2(transform.position.x*fliping, transform.position.y + 1.65f));
        fieldOfView.SetFoV(fov);
        fieldOfView.SetAimDirection(transform.up*fliping);
        fieldOfView.SetViewDistance(viewDistance);
        FindTargetPlayer();
        
        
    }
    private void FixedUpdate()
    {
        Checkflip();
        CheckDistanceAtk();
        DamagePlayer();
    }
        
    public float walk;
    public void StatePatrol()
    {
        if (navEnemy.stopMove == false)
        {

            if (Vector2.Distance(transform.position, patrolPoint.position) < patrolPosition && angry == false)
            {
                chill = true;
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

            else if (goBack == true)
            {
                Goback();
            }
            Attack();
            if (angry == true)
            {
                Angry();
            }
        }
    }
    public void FovActive(bool test)
    {
        if(test) 
        {
            pfFieldOfView.gameObject.SetActive(true);
        }
        else
        {
            pfFieldOfView.gameObject.SetActive(false);  
        }
    }
    private void FindTargetPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            //Player inside ViewDistance
            Vector3 dirToPlayer=(player.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward * fliping, dirToPlayer) < fov/2f)
            {
                //Player inside FOV
                //Angry();
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance);
                if(raycastHit2D.collider != null)
                {
                    if (raycastHit2D.collider.gameObject.GetComponentInParent<PlayerControl>()!=null) 
                    {
                        Attention = true;
                    }
                }
                
                
            }
            
        }
       
    }
    public void stateAnxiety()
    {

        if (Attention)
        {

            navEnemy.stopMove = true;
            if (AttentionTime > 0)
            {
               //State Atention
            }
            else if(AttentionTime == 0)
            {
                //if atenTime = 0 player didnt fled then angry
                //if atenTime = 0 player fled then enemy chill
            }

        }
    }
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
        if (transform.position.x > patrolPoint.position.x)
        {
            moveingRight = false;
        }
        else if (transform.position.x < patrolPoint.position.x)
        {
            moveingRight = true;
        }
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
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    /// FOV

    ///FOV
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(PlayerCheck.position, new Vector2(PlayerCheck.position.x + PlayerCheckRayDistance* transform.localScale.x*fliping, PlayerCheck.position.y));
        Gizmos.color= Color.green;
       
    }

}
