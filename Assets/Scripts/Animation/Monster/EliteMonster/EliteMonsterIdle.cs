using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterIdle : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        prevHP = self.hp;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
        animationHandler += Skill_01_Condition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InjuredCondition(animator, self, prevHP);
        animationHandler(animator, self);
    }
}
