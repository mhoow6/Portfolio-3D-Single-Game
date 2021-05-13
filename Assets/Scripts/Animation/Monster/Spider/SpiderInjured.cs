using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderInjured : CommonMonsterAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchDead(animator);
        SwitchIdle(animator);
    }
}
