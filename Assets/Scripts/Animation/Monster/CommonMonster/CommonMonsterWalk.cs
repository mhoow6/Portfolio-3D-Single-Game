using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterWalk : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prevHP = animator.GetComponent<Monster>().hp;
        self = animator.GetComponent<Monster>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = GameManager.instance.controller.player.transform.position;

        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        InjuredCondition(animator, prevHP, self);
        DeadCondition(animator, self);
        AttackCondition(animator, self);
    }
}
