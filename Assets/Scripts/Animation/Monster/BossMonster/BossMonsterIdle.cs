using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterIdle : BossMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler += Skill_01_Condition;
        animationHandler += Skill_02_Condition;
        animationHandler += Skill_03_Condition;
        damagedStateHandler = null;
    }
}
