using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : StateMachineBehaviour
{
    public float animationDurationTime = 0.25f;
    public float currentAnimationTime;

    public enum AniType {
        IDLE=0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK_01,
        ATTACK_02,
        ARMED,
        UNARMED,
        ROLL,
        COMBAT_IDLE=10,
        COMBAT_WALK,
        COMBAT_INJURED,
        COMBAT_DEAD,
        COMBAT_ATTACK_01,
        COMBAT_ATTACK_02,
        COMBAT_ATTACK_03,
        SKILL_01,
        SKILL_02
    }

    protected void SwitchIdle(Animator animator)
    {
        if (!GameManager.instance.controller.isPlayerWantToMove)
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    protected void SwitchWalk(Animator animator)
    {
        if (GameManager.instance.controller.moveVector.magnitude != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.WALK);
        }
    }

    protected void SwitchAttack_1(Animator animator)
    {
        if (Input.GetAxisRaw("Fire1") != 0)
            animator.SetInteger("ani_id", (int)AniType.ATTACK_01);
    }

    protected void SwitchAttack_2(Animator animator)
    {
        if (Input.GetAxisRaw("Fire1") != 0)
            animator.SetInteger("ani_id", (int)AniType.ATTACK_02);
    }

    protected void SwitchCombatMode(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetInteger("ani_id", (int)AniType.UNARMED);
            GameManager.instance.controller.isCombatMode = false;
            return;
        }

        if (!GameManager.instance.controller.isCombatMode && Input.GetKeyDown(KeyCode.R))
        {
            animator.SetInteger("ani_id", (int)AniType.ARMED);
            GameManager.instance.controller.isCombatMode = true;
            return;
        }
    }

    protected void SwitchCombatModeIdle(Animator animator)
    {
        if (!Input.anyKeyDown && GameManager.instance.controller.isCombatMode)
            animator.SetInteger("ani_id", (int)AniType.COMBAT_IDLE);
    }

    protected void SwitchCombatModeWalk(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && GameManager.instance.controller.moveVector.magnitude != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.COMBAT_WALK);
        }
    }

    protected void SwitchCombatModeAttack(Animator animator)
    {
        if (GameManager.instance.controller.isCombatMode && Input.GetAxisRaw("Fire1") != 0)
        {
            animator.SetInteger("ani_id", (int)AniType.COMBAT_ATTACK_01);
        }
    }

}
