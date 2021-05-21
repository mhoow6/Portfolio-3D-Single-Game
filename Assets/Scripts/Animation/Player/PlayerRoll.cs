using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : PlayerAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRoll = true;
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.currentSp -= GameManager.instance.controller.player.roll_sp;
        GameManager.instance.controller.player.isPlayerNeedSP = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime;

        if (currentAnimationTime > (1 -animationTransitionTime))
        {
            GameManager.instance.controller.isPlayerWantToRoll = false;
            GameManager.instance.controller.immobile = false;
        }
            
        IdleCondition(animator);
        WalkCondition(animator);
        RunCondition(animator);
        DeadCondition(animator);
        Attack_01_Condition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}
