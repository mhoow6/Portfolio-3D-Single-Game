using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboTakeOff : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.isImmortal = true;
        aniHandler = DeadCondition;
    }
}
