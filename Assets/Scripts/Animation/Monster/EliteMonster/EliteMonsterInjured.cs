using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterInjured : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self = GameManager.instance.monsters.Find(mob => mob.index == self.index);
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InjuredCondition(animator, self, prevHP);

        if (self.hp == prevHP)
        {
            IdleCondition(animator, self);
            WalkCondition(animator, self);
            RunCondition(animator, self);
            DeadCondition(animator, self);
            AttackCondition(animator, self);
            Skill_01_Condition(animator, self);
        }

    }
}
