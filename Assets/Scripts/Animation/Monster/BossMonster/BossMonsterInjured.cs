using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterInjured : BossMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.endurance_stack = 0;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // This State doesn't need OnStateUpdate.
    }
}
