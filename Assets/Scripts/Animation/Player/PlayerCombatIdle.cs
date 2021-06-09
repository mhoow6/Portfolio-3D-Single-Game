using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatIdle : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }
    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchCombatModeCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeDeadCondition(animator);
        CombatModeAttack_01_Condition(animator);
        CombatModeSkill_01_Condition(animator);
        CombatModeSkill_02_Condition(animator);
        CombatModeRollCondition(animator);
    }
}
