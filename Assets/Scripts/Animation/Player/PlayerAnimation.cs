using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : StateMachineBehaviour
{
    protected const float animationTransitionTime = 0.25f;
    protected const float interruptAvailableTime = 0.4f;
    protected const float attackClipSpeed = 0.8f;
    protected const float combatAttackClipSpeed = 0.75f;

    [SerializeField]
    protected float currentAnimationTime;

    public enum AniType {
        IDLE=0,
        WALK,
        RUN,
        INJURED, // NO ANIMATION
        DEAD,
        ATTACK_01,
        ATTACK_02,
        ARMED,
        UNARMED,
        ROLL,
        COMBAT_IDLE=10,
        COMBAT_WALK,
        COMBAT_INJURED, // NO ANIMATION
        COMBAT_DEAD,
        COMBAT_ATTACK_01,
        COMBAT_ATTACK_02,
        COMBAT_ATTACK_03,
        COMBAT_SKILL_01,
        COMBAT_SKILL_02,
        COMBAT_ROLL
    }

    protected void IdleCondition(Animator animator)
    {
        if (!GameManager.instance.controller.isPlayerWantToMove)
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    // .controlSlots.Find(slot => slot.name == "Run").isClicked
    protected void WalkCondition(Animator animator)
    {
        if (GameManager.instance.controller.isPlayerWantToMove && !Input.GetKey(KeyCode.LeftShift)
            && !HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Run").isClicked)
            animator.SetInteger("ani_id", (int)AniType.WALK);
    }

    protected void RunCondition(Animator animator)
    {
        if (Input.GetKey(KeyCode.LeftShift) ||
            HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Run").isClicked &&
            GameManager.instance.controller.isPlayerWantToMove &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.run_sp)
                animator.SetInteger("ani_id", (int)AniType.RUN); 
    }

    protected void DeadCondition(Animator animator)
    {
        if (GameManager.instance.controller.player.currentHp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

    protected void Attack_01_Condition(Animator animator)
    {
        if (Input.GetKey(KeyCode.LeftControl) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Attack").isClicked &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.attack_sp)
                animator.SetInteger("ani_id", (int)AniType.ATTACK_01);  
    }

    protected void Attack_02_Condition(Animator animator)
    {
        if (Input.GetKey(KeyCode.LeftControl) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Attack").isClicked &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.attack_sp)
                animator.SetInteger("ani_id", (int)AniType.ATTACK_02);
    }

    protected void SwitchCombatModeCondition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKeyDown(KeyCode.R) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "CombatMode").isClicked))
        {
            animator.SetInteger("ani_id", (int)AniType.UNARMED);
            return;
        }

        if (!GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKeyDown(KeyCode.R) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "CombatMode").isClicked))
        {
            animator.SetInteger("ani_id", (int)AniType.ARMED);
            return;
        }
    }

    protected void RollCondition(Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.Space) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Roll").isClicked &&
            !GameManager.instance.controller.player.isCombatMode &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.roll_sp)
                animator.SetInteger("ani_id", (int)AniType.ROLL); 
    }

    protected void CombatModeIdleCondition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            !GameManager.instance.controller.isPlayerWantToMove)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_IDLE);
    }

    protected void CombatModeWalkCondition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            GameManager.instance.controller.isPlayerWantToMove)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_WALK);
    }

    protected void CombatModeDeadCondition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            GameManager.instance.controller.player.currentHp <= 0)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_DEAD);
    }

    protected void CombatModeAttack_01_Condition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKey(KeyCode.LeftControl) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Attack").isClicked) &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.combat_attack_sp)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_ATTACK_01);
    }

    protected void CombatModeAttack_02_Condition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKey(KeyCode.LeftControl) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Attack").isClicked) &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.combat_attack_sp)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_ATTACK_02);
    }

    protected void CombatModeAttack_03_Condition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKey(KeyCode.LeftControl) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Attack").isClicked) &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.combat_attack_sp)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_ATTACK_03);
    }

    protected void CombatModeSkill_01_Condition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKeyDown(KeyCode.E) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "ESkill").isClicked) &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.skill_01_sp &&
            GameManager.instance.controller.player.currentMp >= PlayerInfoTableManager.playerInfo.skill_01_mp &&
            GameManager.instance.controller.player.current_combat_skill_01_cooldown == 0)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_SKILL_01);
    }

    protected void CombatModeSkill_02_Condition(Animator animator)
    {
        if (GameManager.instance.controller.player.isCombatMode &&
            (Input.GetKeyDown(KeyCode.Q) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "QSkill").isClicked)&&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.skill_02_sp &&
            GameManager.instance.controller.player.currentMp >= PlayerInfoTableManager.playerInfo.skill_02_mp &&
            GameManager.instance.controller.player.current_combat_skill_02_cooldown == 0)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_SKILL_02);
    }

    protected void CombatModeRollCondition(Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.Space) || HUDManager.instance.controlSlotManager.controlSlots.Find(slot => slot.name == "Roll").isClicked &&
            GameManager.instance.controller.player.isCombatMode &&
            GameManager.instance.controller.player.currentSp >= PlayerInfoTableManager.playerInfo.roll_sp)
                animator.SetInteger("ani_id", (int)AniType.COMBAT_ROLL);
    }
}
