using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAttack_01 : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.combat_attack_sp;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * combatAttackClipSpeed;

        if (currentAnimationTime > interruptAvailableTime)
        {
            CombatModeAttack_02_Condition(animator);
            CombatModeSkill_01_Condition(animator);
            CombatModeSkill_02_Condition(animator);
            CombatModeRollCondition(animator);
            return;
        }

        CombatModeWalkCondition(animator);
        CombatModeIdleCondition(animator);
        CombatModeDeadCondition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        GameManager.instance.controller.immobile = false;
        currentAnimationTime = 0;
    }
}
