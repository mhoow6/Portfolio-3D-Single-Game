using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSkill_03 : BossMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isAttackCooldown = true;
            self.thinking_param = (int)AniType.IDLE;
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = 0;
        self.StartCoroutine(self.AttackCooldown(self.attack_duration));
    }
}
