using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboLandReady : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("combo_id", (int)ComboAniType.LAND);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // FlyForward에서 LandReadyCondition 반복 호출로 인해 StateEnter에서 LAND로 바꾸도 다시 LAND READY 상태가 유지됨..
        if (animator.GetInteger("combo_id") != (int)ComboAniType.LAND)
            animator.SetInteger("combo_id", (int)ComboAniType.LAND);

        self.TurnAround(self._TURN_AROUND_SPEED);
    }
}
