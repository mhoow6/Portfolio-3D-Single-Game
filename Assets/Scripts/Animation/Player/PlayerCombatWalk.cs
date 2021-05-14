using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatWalk : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchCombatModeIdle(animator);
        SwitchCombatModeAttack(animator);
        SwitchCombatModeWalk(animator);
    }
}
