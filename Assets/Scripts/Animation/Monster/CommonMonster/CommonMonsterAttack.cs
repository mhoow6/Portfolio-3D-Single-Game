using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterAttack : CommonMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.isMonsterAttackDone = false;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime >= (1 - animationTransitionTime))
            self.isMonsterAttackDone = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = 0;
    }
}
