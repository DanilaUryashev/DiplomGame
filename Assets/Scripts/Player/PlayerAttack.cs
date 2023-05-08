using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;

    public Transform AttackPoint;

    public float attackRange = 0.5f;

    public LayerMask enemyLayer;

    public bool attack;

    public float nextAttackTime;

    public int attackState;

    public float attackDamage = 40f;

    [Header("Fire Gun"), SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float fireAttackDemage;

    private PlayerControl playerCtrl;

    private bool attackReady = true;

    public GameObject impactEffect;

    public LineRenderer lineRenderer;

    private Vector3 mousePosition;

    public float moveSpeed = 2f;

    [Header("Aim body parts"), SerializeField]
    private Transform Hand;

    [SerializeField]
    private Transform Head;

    [SerializeField]
    private Transform Hand_Torsion_center;

    [SerializeField]
    private Transform Head_Torsion_center;

    private bool aiming;

    public float Hand_rotation_z;

    public float Head_rotation_x;

    public float offset;

    public float Head_offset;

    public float Y_Head_offset;

    public float X_Head_offset;

    public float fliper;

    public Vector3 testMouse;

    public Vector3 testDifference;

    public float fasd;

    public Transform traector;

    private void Start()
    {
        playerCtrl = base.GetComponent<PlayerControl>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        if (aiming)
        {
            Aimimg();
        }
    }

    public void Attack()
    {
        nextAttackTime = 0f;
        if (PlayerControl.eqpSwordStatus && attackReady)
        {
            Collider2D[] array = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayer);
            for (int i = 0; i < array.Length; i++)
            {
                array[i].GetComponentInParent<Enemy>().TakeDamage(attackDamage);
            }
            attackReady = false;
            anim.SetTrigger("Attack");
        }
    }

    private void AttackFinish()
    {
        attackReady = true;
    }

    public void fireGun()
    {
        bool arg_06_0 = aiming;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, new Vector2(testDifference.x, testDifference.y));
        hit.transform.GetComponent<PlayerControl>();
        if (hit)
        {
            anim.SetTrigger("FireGun");
            Debug.Log(hit.transform.name);
            Enemy component = hit.transform.GetComponent<Enemy>();
            if (component != null)
            {
                component.TakeDamage(fireAttackDemage);
            }
            Instantiate<GameObject>(impactEffect, hit.point, Quaternion.identity);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        Destroy(impactEffect);
    }

    public void Aim(bool aim)
    {
        aiming = aim;
        anim.SetBool("Aim", aim);
    }

    public void Aimimg()
    {
        Vector3 position = Mouse.current.position.ReadValue();
        position.z = 11f;
        mousePosition = Camera.main.ScreenToWorldPoint(position) - Hand_Torsion_center.position;
        Vector3 vector = mousePosition;
        vector.Normalize();
        testDifference = vector;
        testMouse = mousePosition;
        Hand_rotation_z = Mathf.Atan2(vector.y, vector.x * PlayerControl.fliping) * 57.29578f;
        Head_rotation_x = Mathf.Atan2(vector.y, vector.x * PlayerControl.fliping) * 57.29578f * -1f;
        //Head.transform.rotation = Quaternion.Euler(Head_rotation_x - Head_offset, 90f, 0f);
        //firePoint.transform.rotation = Quaternion.Euler(0f, 0f, Hand_rotation_z - offset);
        if (Head_rotation_x > 40f)
        {
            Head.transform.rotation = Quaternion.Euler(40f, 90f * PlayerControl.fliping, 0f);
        }
        else if (Head_rotation_x < -40f)
        {
            Head.transform.rotation = Quaternion.Euler(-40f, 90f * PlayerControl.fliping, 0f);
        }
        else
        {
            Head.transform.rotation = Quaternion.Euler(Head_rotation_x - Head_offset, 90f * PlayerControl.fliping, 0f);
        }
        if (Hand_rotation_z > 100f || Hand_rotation_z < -100f)
        {
            playerCtrl.flip();
        }
        if (PlayerControl.fliping > 0f)
        {
            Hand.transform.rotation = Quaternion.Euler(0f, 0f, Hand_rotation_z - offset);
        }
        Hand.transform.rotation = Quaternion.Euler(0f, 180f, Hand_rotation_z - offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(traector.position, new Vector2(mousePosition.x, mousePosition.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(firePoint.position, new Vector3(mousePosition.x, mousePosition.y));
    }
}
