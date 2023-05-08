using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public bool crouchKey;

    public bool jumpKey;

    public bool runKey;

    [SerializeField]
    private GameObject TargetObj;

    private PlayerControl playerCtrl;
    private AbilityPlayer playerAbility;
    private OpenMenu setMenu;

    private PlayerAttack playerAttack;

    private bool aiming;

    public void Start()
    {
        playerCtrl = GetComponent<PlayerControl>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAbility= GetComponent<AbilityPlayer>();
    }

    public void OnWalk(InputAction.CallbackContext ctx)
    {
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpKey = true;
            playerCtrl.jump(jumpKey);
        }
        jumpKey = false;
        playerCtrl.jump(jumpKey);
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            crouchKey = true;
            playerCtrl.crouch(crouchKey);
        }
        if (ctx.canceled)
        {
            crouchKey = false;
            playerCtrl.crouch(crouchKey);
        }
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            runKey = true;
            playerCtrl.run(runKey);
        }
        if (ctx.canceled)
        {
            runKey = false;
            playerCtrl.run(runKey);
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!aiming)
            {
                playerAttack.Attack();
    
            }
            if (aiming)
            {
                playerAttack.fireGun();
            }
        }
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aiming = true;
            playerAttack.Aim(aiming);
        }
        if (ctx.canceled)
        {
            aiming = false;
            playerAttack.Aim(aiming);
        }
    }

    public void OnEquipSword(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerCtrl.EquipSword();
        }
    }

    public void OnEscape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerCtrl.openMenu();
        }
    }
    private bool abilActive=false;
    public void OnTab(InputAction.CallbackContext ctx)
    {
        
        if (ctx.performed)
        {
            
            abilActive = true;
            playerAbility.ViewEnemyFOV(abilActive);
        }
        if (ctx.canceled)
        {
            abilActive = false;
            playerAbility.ViewEnemyFOV(abilActive);
        }
    }
}
