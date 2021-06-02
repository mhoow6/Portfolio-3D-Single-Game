using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterInjured : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self.endurance_stack = 0;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += AttackCondition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler(animator, self);
    }
}
