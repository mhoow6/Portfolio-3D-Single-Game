using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRun : PlayerAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = false;
        GameManager.instance.controller.isPlayerWantToRun = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.instance.controller.player.currentSp >= 0)
            GameManager.instance.controller.player.currentSp = Mathf.Lerp(
                GameManager.instance.controller.player.currentSp,
                GameManager.instance.controller.player.currentSp - GameManager.instance.controller.player.run_sp,
                Time.deltaTime * GameManager.instance.controller.player.runningSpReductionRate);  
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

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRun = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
    }
}
