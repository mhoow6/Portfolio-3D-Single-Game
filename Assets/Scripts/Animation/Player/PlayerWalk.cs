using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IdleCondition(animator);
        RunCondition(animator);
        Attack_01_Condition(animator);
        DeadCondition(animator);
        RollCondition(animator);
        SwitchCombatModeCondition(animator);
    }

}
