using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatIdle : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchCombatModeCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeAttack_01_Condition(animator);
        DeadCondition(animator);
    }
}
