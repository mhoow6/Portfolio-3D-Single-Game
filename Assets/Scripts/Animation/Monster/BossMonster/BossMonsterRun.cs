using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterRun : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
        self.agent.speed = self.run_speed;
        self.agent.acceleration = self.run_speed;

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
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
