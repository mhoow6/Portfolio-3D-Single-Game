using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIdle : CommonMonsterAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prevHP = animator.GetComponent<Monster>().hp;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchInjured(animator, prevHP);
        SwitchDead(animator);
    }

}
