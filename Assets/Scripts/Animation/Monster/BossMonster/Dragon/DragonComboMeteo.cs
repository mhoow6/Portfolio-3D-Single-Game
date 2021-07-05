using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboMeteo : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("combo_id", (int)ComboAniType.FLY_AGAIN);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.TurnAround(self._TURN_AROUND_SPEED);
    }
}
