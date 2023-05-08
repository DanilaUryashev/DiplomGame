using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{

    [Header("Main param")]
    [SerializeField] private NavigateEnemy navEnemy;
    // Start is called before the first frame update
    void Start()
    {
        navEnemy = GetComponent<NavigateEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tpbool) ClimbLedgeAnimation();
    }
    public Transform tp;
    public float tpint;
    public LayerMask layerMask;
    void ClimbLedgeAnimation()
    {
        tpint = Physics2D.Raycast(new Vector2(tp.position.x, tp.position.y), Vector2.down, 0.5f, layerMask).distance;
        transform.position = new Vector2(transform.position.x, transform.position.y- tpint);
    }
    bool tpbool;
    void test()
    {
        tpbool = true;
    }
    void test1()
    {
        tpbool = false;
    }

    [SerializeField] Transform AnimHighJumpPoint;
    [SerializeField] float HeigLedgeDistance;
    [SerializeField] private LayerMask Wall;
    ///Animation  JumpLedge
    public void offsetAnimHeigLedge()
    {
        HeigLedgeDistance = Physics2D.Raycast(AnimHighJumpPoint.position, Vector2.down, 1.7f, Wall).distance;
        transform.position = new Vector2(transform.position.x, transform.position.y - HeigLedgeDistance);
    }
    void offsetHighClimb()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.709f);
    }
    void offsetToStand()
    {
        if (navEnemy.onHighLedge)
        {
            transform.position = new Vector2(navEnemy.ToStandUp.position.x, navEnemy.ToStandUp.position.y);
            
            navEnemy.onHighLedge = false;
            navEnemy.onSmallLedge = false;
            navEnemy.onLedge = false;
        }

            
        //if(onHighCliff)
        //    transform.position = new Vector2(ToStandUp.position.x, ToStandUp.position.y);
    }
    void BoolledgeFinish()
    {
        navEnemy.onHighLedge = false;
        navEnemy.onSmallLedge = false;
        navEnemy.onLedge = false;
    }
    void BoolCliffFinish()
    {
        navEnemy.onHighCliff = false;
        navEnemy.onSmallCliff = false;
        navEnemy.onCliff = false;
    }


}
