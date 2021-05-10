using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : StateMachineBehaviour
{
    public enum AniType {
        IDLE=0,
        WALK,
        INJURED,
        DEAD,
        ATTACK,
        ARMED,
        UNARMED,
        COMBAT_IDLE=10,
        COMBAT_WALK,
        COMBAT_INJURED,
        COMBAT_DEAD,
        COMBAT_ATTACK_01,
        COMBAT_ATTACK_02,
        COMBAT_ATTACK_03,
        COMBAT_ATTACK_04,
    }

    protected bool SwitchWalk(Animator animator)
    {
        if (GameManager.instance.controller.moveVector.magnitude != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.WALK);
            return true;
        }
        animator.SetInteger("ani_id", (int)AniType.IDLE);
        return false;      
    }

    protected bool SwitchAttack(Animator animator)
    {
        if (Input.GetAxisRaw("Fire1") != 0) {
            animator.SetInteger("ani_id", (int)AniType.ATTACK);
            return true;
        }
        return false; 
    }

    protected bool SwitchCombatMode(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("combatMode", false);
            GameManager.instance.controller.isCombatMode = false;
            return false;
        }

        if (!GameManager.instance.controller.isCombatMode && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("combatMode", true);
            GameManager.instance.controller.isCombatMode = true;
            return true;
        }

        return false;
    }

    protected bool SwitchCombatModeWalk(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && GameManager.instance.controller.moveVector.magnitude != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.COMBAT_WALK);
            return true;
        }
        animator.SetInteger("ani_id", (int)AniType.COMBAT_IDLE);
        return false;
    }

    protected bool SwitchCombatModeAttack(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && Input.GetAxisRaw("Fire1") != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.COMBAT_ATTACK_01);
            return true;
        }
        return false;
    }

}
