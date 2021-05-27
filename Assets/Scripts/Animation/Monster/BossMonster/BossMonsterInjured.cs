using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterInjured : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InjuredCondition(animator, self, prevHP);
        DeadCondition(animator, self);

        if (self.hp == prevHP)
        {
            IdleCondition(animator, self);
            WalkCondition(animator, self);
            RunCondition(animator, self);
            AttackCondition(animator, self);
            Skill_01_Condition(animator, self);
            Skill_02_Condition(animator, self);
            Skill_03_Condition(animator, self);
        }
    }
}
