using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAttack_03 : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.combat_attack_sp;

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.NONE);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        if (currentAnimationTime > interruptAvailableTime)
        {
            CombatModeAttack_01_Condition(animator);
            CombatModeSkill_01_Condition(animator);
            CombatModeSkill_02_Condition(animator);
            return;
        }

        CombatModeWalkCondition(animator);
        CombatModeIdleCondition(animator);
        CombatModeDeadCondition(animator);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        GameManager.instance.controller.immobile = false;
        currentAnimationTime = 0;
    }
}
