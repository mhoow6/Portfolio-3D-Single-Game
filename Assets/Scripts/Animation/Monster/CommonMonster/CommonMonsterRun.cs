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
        prevHP = self.hp;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = stateInfo.normalizedTime % 2;
        DamagedCondition(animator, self, ref prevHP, ref currentAnimationTime, animationBackTime);

        if (self.agent.enabled == true)
            self.agent.destination = GameManager.instance.controller.player.transform.position;

        if (self.thinking_param == (int)AniType.ATTACK)
            self.agent.destination = self.transform.position;

        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
