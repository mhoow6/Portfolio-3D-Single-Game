using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterInjured : EliteMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.endurance_stack = 0;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        self.thinking_param = (int)AniType.IDLE;
        self.isStuned = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.StartCoroutine(self.StunCooldown(self.stun_escape));
    }
}
