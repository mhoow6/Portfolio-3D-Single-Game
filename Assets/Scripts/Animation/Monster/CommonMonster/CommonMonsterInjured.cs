using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterInjured : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
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
        }
            
    }
}
