using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchWalk(animator);
        SwitchAttack_1(animator);
        SwitchCombatMode(animator);
    }

}
