using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnArmed : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isCombatMode = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ani_id", (int)AniType.IDLE);
        DeadCondition(animator);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
    }
}
