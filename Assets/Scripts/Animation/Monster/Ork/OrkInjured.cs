using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrkInjured : EliteMonsterAnimation
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchDead(animator);
        SwitchIdle(animator);
    }
}
