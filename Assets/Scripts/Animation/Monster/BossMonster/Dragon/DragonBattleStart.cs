using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleStart : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = BattleStart;
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.bossCam.CompleteCamMove();
    }
}
