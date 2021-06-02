using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterAttack : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self.isMonsterAttackDone = false;
        animationHandler = IdleCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = stateInfo.normalizedTime;
        currentAnimationTime = Mathf.Clamp(currentAnimationTime, 0f, 1f);
        DamagedCondition(animator, self, ref prevHP, ref currentAnimationTime, animationBackTime);

        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isMonsterAttackDone = true;
            //currentAnimationTime = 0;
        }

        animationHandler(animator, self);
    }
}
