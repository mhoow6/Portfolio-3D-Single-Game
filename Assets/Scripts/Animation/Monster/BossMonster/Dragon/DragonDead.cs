using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDead : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.Dead();
        self.hp = 0;
        self.endurance_stack = 0;
        aniHandler = null;

        HUDManager.instance.boss_state.gameObject.SetActive(false);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.hp = 0;
        self.endurance_stack = 0;
    }
}
