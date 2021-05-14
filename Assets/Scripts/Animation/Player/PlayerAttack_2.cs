using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_2 : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 - animationDurationTime)
        {
            SwitchWalk(animator);
            SwitchIdle(animator);
            SwitchAttack_1(animator);
            GameManager.instance.controller.immobile = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
