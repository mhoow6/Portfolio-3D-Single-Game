using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnArmed : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isCombatMode = false;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        
        animator.SetInteger("ani_id", (int)AniType.IDLE);
        DeadCondition(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }
}
