using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboMeteo : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("combo_id", (int)ComboAniType.FLY_AGAIN);
        fireballSource = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.ALL, AudioCondition.DRAGON_FIREBALL), AudioManager.instance._WALK_SOUND);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.instance.StopAudioFadeOut(fireballSource);
        self.TurnAround(self._TURN_AROUND_SPEED);
    }
}
