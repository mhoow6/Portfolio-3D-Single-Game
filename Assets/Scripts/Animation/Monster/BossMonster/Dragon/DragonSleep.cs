using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSleep : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameManager.instance.controller.player;

        if (self == null)
            self = animator.GetComponent<Dragon>();

        aniHandler = OnPlayerEncounter;
    }
}
