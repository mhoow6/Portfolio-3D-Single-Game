using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmed : PlayerAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isCombatMode = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;

        animator.SetInteger("ani_id", (int)AniType.COMBAT_IDLE);
        DeadCondition(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }
}
