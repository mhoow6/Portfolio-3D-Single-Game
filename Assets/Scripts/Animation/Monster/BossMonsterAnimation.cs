using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterAnimation : MonsterAnimation
{
    public enum AniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK,
        SKILL_1,
        SKILL_2,
        SKILL_3,
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
        if (animator.GetComponent<Monster>().hp < prevHP && animator.GetComponent<Monster>().hp > 0)
            animator.SetInteger("ani_id", (int)AniType.INJURED);
    }

    protected override void SwitchDead(Animator animator)
    {
        if (animator.GetComponent<Monster>().hp <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

    protected void SwitchSkill_1(Animator animator)
    {
        /* Need Implementation */
    }

    protected void SwitchSkill_2(Animator animator)
    {
        /* Need Implementation */
    }

    protected void SwitchSkill_3(Animator animator)
    {
        /* Need Implementation */
    }
}
