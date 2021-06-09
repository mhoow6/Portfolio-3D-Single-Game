using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterIdle : CommonMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationHandler = IdleCondition;
        animationHandler += WalkCondition;
        animationHandler += RunCondition;
        animationHandler += InjuredCondition;
        animationHandler += DeadCondition;
        animationHandler += AttackCondition;
    }
}
