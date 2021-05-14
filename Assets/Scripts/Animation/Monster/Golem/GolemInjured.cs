using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemInjured : BossMonsterAnimation
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchIdle(animator);
    }
}
