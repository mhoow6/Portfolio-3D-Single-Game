using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterAnimation : MonsterAnimation
{
    public enum BossMobAniType
    {
        SKILL_1 = 6,
        SKILL_2,
        SKILL_3,
    }

    protected override void IdleCondition(Animator animator, Monster monster)
    {
        throw new System.NotImplementedException();
    }

    protected override void WalkCondition(Animator animator, Monster monster)
    {
        throw new System.NotImplementedException();
    }

    protected override void RunCondition(Animator animator, Monster monster)
    {
        throw new System.NotImplementedException();
    }

    protected override void InjuredCondition(Animator animator, Monster monster, float prevHP)
    {
        throw new System.NotImplementedException();
    }

    protected override void DeadCondition(Animator animator, Monster monster)
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackCondition(Animator animator, Monster monster)
    {
        throw new System.NotImplementedException();
    }

    protected void Skill_1_Condition(Animator animator)
    {
        /* Need Implementation */
    }

    protected void Skill_2_Condition(Animator animator)
    {
        /* Need Implementation */
    }

    protected void Skill_3_Condition(Animator animator)
    {
        /* Need Implementation */
    }
}
