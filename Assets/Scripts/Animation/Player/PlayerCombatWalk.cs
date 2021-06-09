using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatWalk : PlayerAnimation
{
    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;

        CombatModeIdleCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeDeadCondition(animator);
        CombatModeRollCondition(animator);
        CombatModeAttack_01_Condition(animator);
        CombatModeSkill_01_Condition(animator);
        CombatModeSkill_02_Condition(animator);
    }
}
