using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    
    public Transform AttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    public float AttackRate = 2f;
    public float nextAttackTime = 0f;
    public int attackState = 0;
    
    
    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (attackReady)
        {
            if (Input.GetMouseButtonDown(0)&& PlayerControl.eqpSwordStatus)
            {
                Attack();
               // nextAttackTime = Time.time + 2f / AttackRate;
            }
        }
        fireGun();



    }
    public float attackDamage=40f;

   
    void Attack()
    {
        attackState += 1;
        if (attackState == 1 && attackReady)
        {
            anim.SetTrigger("Attack1");
            
        }
        else if (attackState == 2&&attackReady)
        {
            anim.SetTrigger("Attack2");
           
        }
        else if (attackState == 3 && attackReady)
        {
            anim.SetTrigger("Attack3");
            attackState =0;
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponentInParent<Enemy>().TakeDamage(attackDamage);
         
        }
        attackReady = false;
    }
    private bool attackReady = true;
    void AttackFinish()
    {
        attackReady = true;
    }
    public Transform bulletspawn;
    public Rigidbody2D rbBullet;
    void fireGun()
    {
        
        if (Input.GetMouseButton(1))
        {
            anim.SetTrigger("FireGun");
            rbBullet.AddForce((Vector2.right * 1f * PlayerControl.fliping * 100));

        }
    }





     void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
