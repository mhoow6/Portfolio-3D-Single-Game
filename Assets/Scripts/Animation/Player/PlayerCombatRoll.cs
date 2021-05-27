using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatRoll : PlayerAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false; // ������ �Է� �� ���������� ������ �ٲ� �� �ִ�.
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.roll_sp;
        currentAnimationTime = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
        {
            GameManager.instance.controller.immobile = true;
            GameManager.instance.controller.isPlayerWantToRoll = true;
            GameManager.instance.controller.player.isPlayerNeedSP = false;
            currentAnimationTime += Time.deltaTime;
        }
            
        CombatModeIdleCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeDeadCondition(animator);
        CombatModeAttack_01_Condition(animator);
        CombatModeSkill_01_Condition(animator);
        CombatModeSkill_02_Condition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRoll = false;
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}