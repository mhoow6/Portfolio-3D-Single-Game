using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : StateMachineBehaviour
{
    public enum BasicAniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK
    }

    protected float prevHP;
    protected Monster self;

    protected void IdleCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)BasicAniType.IDLE)
            animator.SetInteger("ani_id", (int)BasicAniType.IDLE);
    }

    protected void WalkCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)BasicAniType.WALK)
            animator.SetInteger("ani_id", (int)BasicAniType.WALK);
    }

    protected void RunCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)BasicAniType.RUN)
            animator.SetInteger("ani_id", (int)BasicAniType.RUN);
    }

    protected void InjuredCondition(Animator animator, float prevHP, Monster monster)
    {
        if (monster.hp < prevHP && animator.GetComponent<Monster>().hp > 0)
            animator.SetInteger("ani_id", (int)BasicAniType.INJURED);
    }

    protected void DeadCondition(Animator animator, Monster monster)
    {
        if (monster.hp <= 0)
            animator.SetInteger("ani_id", (int)BasicAniType.DEAD);
    }

    protected void AttackCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)BasicAniType.ATTACK)
            animator.SetInteger("ani_id", (int)BasicAniType.ATTACK);
    }
}
