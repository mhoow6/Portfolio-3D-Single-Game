using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterRun : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self.agent.speed = self.run_speed;
        self.agent.acceleration = self.run_speed;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = GameManager.instance.controller.player.transform.position;

        InjuredCondition(animator, self, prevHP);
        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
