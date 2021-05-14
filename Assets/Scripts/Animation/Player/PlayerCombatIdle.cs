using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatIdle : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchCombatMode(animator);
        SwitchCombatModeWalk(animator);
        SwitchCombatModeAttack(animator);
    }
}
