using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterInjured : BossMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.endurance_stack = 0;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.thinking_param = (int)AniType.IDLE;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.isStuned = true;
        self.StartCoroutine(self.StunCooldown(self.stun_escape));
    }
}
