using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleFireBall : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
        aniHandler = DeadCondition;

        fireballSource = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.ALL, AudioCondition.DRAGON_FIREBALL), AudioManager.instance._DRAGON_FIREBALL_SOUND);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime >= (1 - defaultAnimationTransitionTime))
        {
            self.isFireBallCooldown = true;
            self.isFreeze = true;
            animator.SetInteger("ani_id", (int)AniType.IDLE);
        }

    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = 0;
        AudioManager.instance.StopAudioFadeOut(fireballSource);

        self.StartCoroutine(self.FireBallCooldown());
        self.StartCoroutine(self.FireBallAfterFreeze());
    }
}
