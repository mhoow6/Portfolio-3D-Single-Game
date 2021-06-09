using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRoll = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.roll_sp;
        currentAnimationTime = 0;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
        {
            GameManager.instance.controller.immobile = true; // 구르기 도중에는 불가능
        }

        IdleCondition(animator);
        WalkCondition(animator);
        RunCondition(animator);
        DeadCondition(animator);
        Attack_01_Condition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRoll = false;
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}
