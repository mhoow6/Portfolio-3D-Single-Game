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

    [SerializeField]
    protected float currentAnimationTime;
    protected float prevHP;
    protected const float animationBackTime = 0.1f;

    // Factory Method
    protected virtual void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<Monster>();
        prevHP = self.hp;

        StateEnter(animator, stateInfo, layerIndex);
    }

    // Factory Method
    protected virtual void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = stateInfo.normalizedTime % 1;
        animationHandler(animator, self);
        DamagedCondition(animator, self, ref prevHP, currentAnimationTime, animationBackTime);

        StateUpdate(animator, stateInfo, layerIndex);
    }

    protected void IdleCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.IDLE && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    protected void WalkCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.WALK && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.WALK);
    }

    protected void RunCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.RUN && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.RUN);
    }

    protected void InjuredCondition(Animator animator, Monster monster)
    {
        if (monster.endurance_stack >= monster.endurance && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.INJURED);
    }

    protected void DeadCondition(Animator animator, Monster monster)
    {
        if (monster.hp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

    protected void AttackCondition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.ATTACK && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.ATTACK);
    }

    protected void DamagedCondition(Animator animator, Monster monster, ref float prevHP, float currentAnimationTime, float animationBackTime)
    {
        if (monster.hp < prevHP && monster.hp > 0 && monster.endurance_stack < monster.endurance)
        {
            prevHP = monster.hp;
            animator.Play(0, 0, currentAnimationTime - animationBackTime);
        }
    }
}
