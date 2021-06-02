using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterSkill_01 : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self.isMonsterAttackDone = false;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += stateInfo.normalizedTime;

        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isMonsterAttackDone = true;
            currentAnimationTime = 0;
        }

        animationHandler(animator, self);
    }
}
