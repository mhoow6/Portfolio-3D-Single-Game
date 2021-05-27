using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterWalk : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
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
        DeadCondition(animator, self);
        AttackCondition(animator, self);
        Skill_01_Condition(animator, self);
        Skill_02_Condition(animator, self);
        Skill_03_Condition(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
