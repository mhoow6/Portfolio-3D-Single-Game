using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchWalk(animator);
        SwitchAttack(animator);
        SwitchCombatMode(animator);
    }
}
