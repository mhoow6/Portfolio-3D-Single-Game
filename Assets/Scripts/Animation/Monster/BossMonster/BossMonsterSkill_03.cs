using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSkill_03 : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
        self.isMonsterAttackDone = false;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * skill_03_ClipSpeed;

        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isMonsterAttackDone = true;
            currentAnimationTime = 0;
        }

        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        DeadCondition(animator, self);
    }
}
