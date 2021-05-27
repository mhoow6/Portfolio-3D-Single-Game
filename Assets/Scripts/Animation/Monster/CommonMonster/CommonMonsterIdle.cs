using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterIdle : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        InjuredCondition(animator, self, prevHP);
        DeadCondition(animator, self);
        AttackCondition(animator, self);
    }
}
