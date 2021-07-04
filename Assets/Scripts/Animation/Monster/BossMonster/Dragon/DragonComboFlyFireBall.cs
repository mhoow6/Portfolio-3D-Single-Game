using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboFlyFireBall : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("combo_id", (int)ComboAniType.FLY);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.TurnAround(TURN_AROUND_SPEED * Time.deltaTime);
    }
}
