using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterIdle : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = stateInfo.normalizedTime % 2;
        DamagedCondition(animator, self, ref prevHP, ref currentAnimationTime, animationBackTime);

        animationHandler(animator, self);
    }
}
