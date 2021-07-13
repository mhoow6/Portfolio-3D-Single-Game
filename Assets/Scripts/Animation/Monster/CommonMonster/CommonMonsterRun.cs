using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterRun : CommonMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.agent.enabled == true)
        {
            self.agent.speed = self.run_speed;
            self.agent.acceleration = self.run_speed;
        }
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.agent.enabled == true)
        {
            self.agent.destination = GameManager.instance.controller.player.transform.position;
            if (self.thinking_param == (int)AniType.ATTACK)
                self.agent.destination = self.transform.position;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
