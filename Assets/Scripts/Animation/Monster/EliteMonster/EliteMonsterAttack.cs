using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterAttack : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self.isMonsterAttackDone = false;
        prevHP = self.hp;
        animationHandler -= AttackCondition;
        animationHandler -= Skill_01_Condition;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * attackClipSpeed;

        if (currentAnimationTime >= (1 - animationTransitionTime))
        {
            self.isMonsterAttackDone = true;
            currentAnimationTime = 0;
        }

        InjuredCondition(animator, self, prevHP);
        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler += AttackCondition;
        animationHandler += Skill_01_Condition;
    }
}
