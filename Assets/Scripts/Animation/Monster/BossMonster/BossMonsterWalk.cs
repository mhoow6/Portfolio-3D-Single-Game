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
        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
