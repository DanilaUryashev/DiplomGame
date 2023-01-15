using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;
    public Image bar;
    public float fill;
    // Start is called before the first frame update
    void Start()
    {
        fill = playerHealth*0.01f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
     

    }
    public float damage = 1f;

    // Update is called once per frame
    void Update()
    {
        
        bar.fillAmount = fill;
        PlayerHealth();
        Damage();
       

    }
    private void FixedUpdate()
    {
        Fail();
    }
    public static float playerHealth=100f;

    public bool death = false;
    void PlayerHealth()
    {
        if (playerHealth <= 0)
        {
            death = true;
            anim.SetBool("Death", death);
        }
        else death = false;
    }
    void Fail()
    {
        if (death)
        {
            
           
            transform.position = new Vector3(respawn.position.x, respawn.position.y, respawn.position.z);
            PlayerControl.blockMoveXYforLedge = true;
            PlayerControl.jumpLock = true;
            playerHealth = 100f;
            rb.gravityScale = 1;

        }
    }

    public Transform respawn;
    void Damage()
    {
        playerHealth -= Time.deltaTime * damage;
        fill = playerHealth * 0.01f;
    }
    //public Transform Interface;
    //public Transform Cam;
    //void TransformInterface()
    //{
    //    Interface.position = new Vector3(Cam.position.x, Cam.position.y, Cam.position.z + 7);
    //}

}

