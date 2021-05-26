using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterSkill_01 : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self = GameManager.instance.monsters.Find(mob => mob.index == self.index);
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        InjuredCondition(animator, self, prevHP);
        DeadCondition(animator, self);

        if (self.hp == prevHP)
        {
            AttackCondition(animator, self);
            Skill_01_Condition(animator, self);
        }
            
    }
}
