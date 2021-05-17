using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAttack : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        SwitchCombatModeWalk(animator);
        SwitchCombatModeIdle(animator);
        SwitchCombatModeAttack(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }
}
