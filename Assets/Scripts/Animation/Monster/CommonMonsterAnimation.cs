using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterAnimation : MonsterAnimation
{
    public enum AniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK
    }

    protected override void SwitchIdle(Animator animator)
    {
        animator.SetInteger("ani_id", (int)AniType.IDLE);
    }

    protected override void SwitchWalk(Animator animator)
    {
        /* Need Implementation */
        throw new System.NotImplementedException();
    }

    protected override void SwitchRun(Animator animator)
    {
        /* Need Implementation */
        throw new System.NotImplementedException();
    }

    protected override void SwitchAttack(Animator animator)
    {
        /* Need Implementation */
        throw new System.NotImplementedException();
    }

    protected override void SwitchInjured(Animator animator, float prevHP)
    {
        if (animator.GetComponent<Monster>().hp < prevHP)
            animator.SetInteger("ani_id", (int)AniType.INJURED);
    }

    protected override void SwitchDead(Animator animator)
    {
        if (animator.GetComponent<Monster>().hp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

}
