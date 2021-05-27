using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAttack : CommonMonsterAnimation
{
    private float pigAttackClipSpeed = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self = GameManager.instance.monsters.Find(mob => self.index == mob.index);
        self.isMonsterAttackDone = false;
        prevHP = self.hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * pigAttackClipSpeed;

        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isMonsterAttackDone = true;
            currentAnimationTime = 0;
        }

        IdleCondition(animator, self);
        WalkCondition(animator, self);
        RunCondition(animator, self);
        InjuredCondition(animator, self, prevHP);
        DeadCondition(animator, self);
    }
}
