using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleStart : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = BattleStart;
    }
}
