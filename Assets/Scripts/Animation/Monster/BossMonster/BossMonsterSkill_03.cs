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
        animationHandler -= AttackCondition;
        animationHandler -= Skill_01_Condition;
        animationHandler -= Skill_02_Condition;
        animationHandler -= Skill_03_Condition;
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

        animationHandler(animator, self);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler += AttackCondition;
        animationHandler += Skill_01_Condition;
        animationHandler += Skill_02_Condition;
        animationHandler += Skill_03_Condition;
    }
}
