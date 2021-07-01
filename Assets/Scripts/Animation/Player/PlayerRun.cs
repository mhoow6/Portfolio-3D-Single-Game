using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = false;
        GameManager.instance.controller.isPlayerWantToRun = true;
        GameManager.instance.controller.immobile = false;

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.RUN);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.instance.controller.player.currentSp >= 0)
            GameManager.instance.controller.player.currentSp = Mathf.Lerp(
                GameManager.instance.controller.player.currentSp,
                GameManager.instance.controller.player.currentSp - PlayerInfoTableManager.playerInfo.run_sp,
                Time.deltaTime * PlayerInfoTableManager.playerInfo.running_sp_reduction_rate);  
        else
        {
            GameManager.instance.controller.player.currentSp = 0;
            animator.SetInteger("ani_id", (int)AniType.WALK);
        }


        IdleCondition(animator);
        WalkCondition(animator);
        DeadCondition(animator);
        Attack_01_Condition(animator);
        RollCondition(animator);

    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRun = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
    }
}
