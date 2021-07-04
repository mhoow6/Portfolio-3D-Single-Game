using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleTurnAround : DragonAnimation
{
    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.TurnAround(TURN_AROUND_SPEED * Time.deltaTime);

        if (currentAnimationTime >= (1 - defaultAnimationTransitionTime))
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }
}