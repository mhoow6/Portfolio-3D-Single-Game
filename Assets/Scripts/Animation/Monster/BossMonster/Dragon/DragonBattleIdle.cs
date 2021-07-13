using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleIdle : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = WalkCondition;
        aniHandler += AttackCondition;
        aniHandler += TailAttackCondition;
        aniHandler += TurnAroundCondition;
        aniHandler += FireBallCondition;
        aniHandler += SpecialComboCondition;
        aniHandler += DeadCondition;
    }
}
