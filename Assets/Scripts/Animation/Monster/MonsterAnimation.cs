using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MonsterAnimationHandler(Animator animator, Monster monster);

public class MonsterAnimation : StateMachineBehaviour
{
    public enum AniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK,
        SKILL_01,
        SKILL_02,
        SKILL_03
    }

    protected MonsterAnimationHandler animationHandler;
    protected Monster self;
    protected float prevHP;
    protected const float animationBackTime = 0.1f;
    

    protected virtual void IdleCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.IDLE && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    protected virtual void WalkCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.WALK && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.WALK);
    }

    protected virtual void RunCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.RUN && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.RUN);
    }

    protected virtual void InjuredCondition(Animator animator, Monster monster)
    {
        if (monster.endurance_stack >= monster.endurance && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.INJURED);
    }

    protected virtual void DeadCondition(Animator animator, Monster monster)
    {
        if (monster.hp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

    protected virtual void AttackCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.ATTACK && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.ATTACK);
    }

    protected virtual void DamagedCondition(Animator animator, Monster monster, ref float prevHP, ref float currentAnimationTime, float animationBackTime)
    {
        if (monster.hp < prevHP && monster.hp > 0)
        {
            prevHP = monster.hp;
            animator.Play(0, 0, currentAnimationTime - animationBackTime);
        }
    }
}
