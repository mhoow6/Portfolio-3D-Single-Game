using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterAnimation : MonsterAnimation
{
    protected const float animationTransitionTime = 0.25f;
    protected const float attackClipSpeed = 1f;
    protected const float skill_01_ClipSpeed = 1f;
    protected const float skill_02_ClipSpeed = 0.5f;
    protected const float skill_03_ClipSpeed = 1f;

    [SerializeField]
    protected float currentAnimationTime;

    protected void Skill_01_Condition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.SKILL_01 && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.SKILL_01);
    }

    protected void Skill_02_Condition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.SKILL_02 && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.SKILL_02);
    }

    protected void Skill_03_Condition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.SKILL_03 && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.SKILL_03);
    }
}
