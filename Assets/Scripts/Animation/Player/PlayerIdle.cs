using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WalkCondition(animator);
        RunCondition(animator);
        Attack_01_Condition(animator);
        DeadCondition(animator);
        SwitchCombatModeCondition(animator);
    }

}
