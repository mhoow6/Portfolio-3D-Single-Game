using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigInjured : CommonMonsterAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchIdle(animator);
    }
}
