using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterAnimation : MonsterAnimation
{
    public enum AniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK,
        SKILL_01
    }

    protected override void IdleCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.IDLE)
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    protected override void WalkCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.WALK)
            animator.SetInteger("ani_id", (int)AniType.WALK);
    }

    protected override void RunCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.RUN)
            animator.SetInteger("ani_id", (int)AniType.RUN);
    }

    protected override void InjuredCondition(Animator animator, Monster monster, float prevHP)
    {
        if (monster.hp < prevHP && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.INJURED);
    }

    protected override void DeadCondition(Animator animator, Monster monster)
    {
        if (monster.hp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

    protected override void AttackCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.ATTACK)
            animator.SetInteger("ani_id", (int)AniType.ATTACK);
    }

    protected void Skill_01_Condition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.SKILL_01)
            animator.SetInteger("ani_id", (int)AniType.SKILL_01);
    }
}
