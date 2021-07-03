using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleWalk : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = WalkCondition;
        aniHandler += AttackCondition;
        aniHandler += FireBallCondition;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = player.transform.position;
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
    }
}
