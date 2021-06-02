using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterRun : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self.agent.speed = self.run_speed;
        self.agent.acceleration = self.run_speed;
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
