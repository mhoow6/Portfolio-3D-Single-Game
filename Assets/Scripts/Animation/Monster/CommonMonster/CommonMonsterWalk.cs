using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterWalk : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self = GameManager.instance.monsters.Find(mob => mob.index == self.index);
        self.agent.speed = self.walk_speed;
        self.agent.acceleration = self.walk_speed;

        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = GameManager.instance.controller.player.transform.position;

        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        InjuredCondition(animator, self, prevHP);
        DeadCondition(animator, self);
        AttackCondition(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position; // ∞»±‚ ¡æ∑·Ω√ ∏ÿ√„
    }
}
