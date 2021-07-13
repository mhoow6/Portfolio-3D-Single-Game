using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerAnimation
{

    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.WALK);

        walkSource = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.ALL, AudioCondition.PLAYER_WALK), AudioManager.instance._WALK_SOUND);
        walkSource.loop = true;

        GameManager.instance.controller.immobile = false;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IdleCondition(animator);
        RunCondition(animator);
        Attack_01_Condition(animator);
        DeadCondition(animator);
        RollCondition(animator);
        SwitchCombatModeCondition(animator);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        walkSource.Stop();
    }

}
