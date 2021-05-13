using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrkIdle : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prevHP = animator.GetComponent<Monster>().hp;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SwitchInjured(animator, prevHP);
        SwitchDead(animator);
    }
}
