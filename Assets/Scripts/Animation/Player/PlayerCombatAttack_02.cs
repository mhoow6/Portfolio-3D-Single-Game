using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAttack_02 : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.currentSp -= GameManager.instance.controller.player.combat_attack_sp;
        GameManager.instance.controller.player.isPlayerNeedSP = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * combatAttackClipSpeed;

        if (currentAnimationTime > attackAvailableTime)
        {
            CombatModeAttack_03_Condition(animator);
            return;
        }
            
        CombatModeWalkCondition(animator);
        CombatModeIdleCondition(animator);
        DeadCondition(animator);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}
