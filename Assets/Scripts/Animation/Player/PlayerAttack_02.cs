using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_02 : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.attack_sp;

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.NONE);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        if (currentAnimationTime > interruptAvailableTime)
        {
            Attack_01_Condition(animator);
            RollCondition(animator);
            return;
        }

        IdleCondition(animator);
        WalkCondition(animator);
        RunCondition(animator);
        DeadCondition(animator);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        GameManager.instance.controller.immobile = false;
        currentAnimationTime = 0;
    }
}
