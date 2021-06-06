using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterWalk : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self.agent.speed = self.walk_speed;
        self.agent.acceleration = self.walk_speed;
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = stateInfo.normalizedTime % 2;
        DamagedCondition(animator, self, ref prevHP, ref currentAnimationTime, animationBackTime, "Base Layer.Walk1");

        if (self.agent.enabled == true)
            self.agent.destination = GameManager.instance.controller.player.transform.position;

        if (self.thinking_param == (int)AniType.ATTACK)
            self.agent.destination = self.transform.position;

        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.agent.enabled == true)
            self.agent.destination = self.transform.position; // ∞»±‚ ¡æ∑·Ω√ ∏ÿ√„
    }
}
