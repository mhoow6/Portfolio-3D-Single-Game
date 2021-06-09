using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterAnimation : MonsterAnimation
{
    protected const float animationTransitionTime = 0.25f;
    protected const float attackClipSpeed = 0.7f;
    protected const float skillClipSpeed = 0.7f;

    protected void Skill_01_Condition(Animator animator, Monster monster)
    {
        if (monster.thinking_param == (int)AniType.SKILL_01 && monster.hp > 0)
            animator.SetInteger("ani_id", (int)AniType.SKILL_01);
    }
}
