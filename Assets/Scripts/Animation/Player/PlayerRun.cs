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
        IdleCondition(animator);
        WalkCondition(animator);
        Attack_01_Condition(animator);

        GameManager.instance.controller.player.currentSp = Mathf.Lerp(
            GameManager.instance.controller.player.currentSp,
            GameManager.instance.controller.player.currentSp - GameManager.instance.controller.player.run_sp,
            Time.deltaTime);

        DeadCondition(animator);
        RollCondition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.isPlayerWantToRun = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
    }
}
