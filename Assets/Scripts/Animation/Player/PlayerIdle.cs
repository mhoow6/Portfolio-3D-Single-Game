using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        WalkCondition(animator);
        RunCondition(animator);
        Attack_01_Condition(animator);
        DeadCondition(animator);
        SwitchCombatModeCondition(animator);
    }
}
