using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterIdle : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
        prevHP = self.hp;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
        animationHandler += Skill_01_Condition;
        animationHandler += Skill_02_Condition;
        animationHandler += Skill_03_Condition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler(animator, self);
    }
}
