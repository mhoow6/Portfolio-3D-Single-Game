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
        // FlyForward���� LandReadyCondition �ݺ� ȣ��� ���� StateEnter���� LAND�� �ٲٵ� �ٽ� LAND READY ���°� ������..
        if (animator.GetInteger("combo_id") != (int)ComboAniType.LAND)
            animator.SetInteger("combo_id", (int)ComboAniType.LAND);

        self.TurnAround(self._TURN_AROUND_SPEED);
    }
}
