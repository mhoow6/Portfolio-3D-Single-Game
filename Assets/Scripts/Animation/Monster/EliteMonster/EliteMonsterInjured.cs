using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterInjured : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self.endurance_stack = 0;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += AttackCondition;
        animationHandler += Skill_01_Condition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler(animator, self);
    }
}
