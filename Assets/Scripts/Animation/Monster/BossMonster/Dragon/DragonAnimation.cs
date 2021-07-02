using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DragonAnimationHandler(Animator animator, Dragon dragon);

public class DragonAnimation : StateMachineBehaviour
{
    protected static Dragon self;
    protected static Player player;
    protected DragonAnimationHandler aniHandler;
    protected float prevHP;
    protected float currentAnimationTime;

    protected const float defaultAnimationTransitionTime = 0.25f;
    protected const float SPARE_DISTANCE = 1f;

    public enum AniType
    {
        NONE = -1,
        IDLE = 0,
        WALK,
        ATTACK,
        TAIL_ATTACK,
        FIREBALL,
        INJURED
    }

    public enum ComboAniType
    {
        NONE = -1,
        TAKEOFF = 0,
        FLY,
        FLY_FIREBALL,
        METEO,
        FLY_AGAIN,
        FLY_FORWARD,
        LAND_READY,
        LAND
    }

    protected bool isEncounted;
    protected bool isBattleStart;
    protected bool specialCombo;

    protected virtual void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    protected virtual void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    protected virtual void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self != null)
            prevHP = self.hp;
        
        StateEnter(animator, stateInfo, layerIndex);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aniHandler != null)
            aniHandler(animator, self);

        StateUpdate(animator, stateInfo, layerIndex);

        currentAnimationTime = stateInfo.normalizedTime % 1;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateExit(animator, stateInfo, layerIndex);
    }

    protected void OnPlayerEncounter(Animator animator, Dragon dragon)
    {
        if (dragon._currentDistanceWithPlayer <= dragon._detectRange)
        {
            isEncounted = true;
            animator.SetBool("isEncounted", true);
        }
    }

    protected void BattleStart(Animator animator, Dragon dragon)
    {
        isBattleStart = true;
        animator.SetBool("isBattleStart", true);
    }

    protected void WalkCondition(Animator animator, Dragon dragon)
    {
        if (player.currentHp > 0)
        {
            if (dragon._currentDistanceWithPlayer >= dragon._attackRange + SPARE_DISTANCE)
                animator.SetInteger("ani_id", (int)AniType.WALK);
        }

        
    }

    protected void AttackCondition(Animator animator, Dragon dragon)
    {
        if (player.currentHp > 0)
        {
            if (!dragon.isAttackCooldown)
            {
                if (dragon._currentDistanceWithPlayer < dragon._attackRange && dragon._currentAngleWithPlayer < dragon._attackAngle)
                    animator.SetInteger("ani_id", (int)AniType.ATTACK);
            }
        }
    }

    protected void TailAttackCondition(Animator animator, Dragon dragon)
    {
        if (player.currentHp > 0)
        {
            if (!dragon.isAttackCooldown)
            {
                if (dragon.IsTailAttackHitable())
                    if (dragon._currentDistanceWithPlayer < dragon._tailAttackRange)
                        animator.SetInteger("ani_id", (int)AniType.TAIL_ATTACK);
            }
        }
        
    }
}
