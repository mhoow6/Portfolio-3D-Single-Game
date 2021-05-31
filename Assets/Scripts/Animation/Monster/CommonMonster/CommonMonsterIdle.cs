using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterIdle : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        prevHP = self.hp;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler(animator, self);
        InjuredCondition(animator, self, prevHP);
    }
}
