using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatRoll : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false; // 구르기 입력 시 순간적으로 방향을 바꿀 수 있다.
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.roll_sp;
        currentAnimationTime = 0;

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.NONE);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
        {
            GameManager.instance.controller.immobile = true;
            GameManager.instance.controller.isPlayerWantToRoll = true;
            GameManager.instance.controller.player.isPlayerNeedSP = false;
        }
            
        CombatModeIdleCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeDeadCondition(animator);
        CombatModeAttack_01_Condition(animator);
        CombatModeSkill_01_Condition(animator);
        CombatModeSkill_02_Condition(animator);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRoll = false;
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}
