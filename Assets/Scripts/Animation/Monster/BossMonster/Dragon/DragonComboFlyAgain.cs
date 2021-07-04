using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboFlyAgain : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("combo_id", (int)ComboAniType.FLY_FORWARD);
    }
}
