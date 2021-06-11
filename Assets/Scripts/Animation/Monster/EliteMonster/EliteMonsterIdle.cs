using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterIdle : EliteMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler += Skill_01_Condition;
    }
}
