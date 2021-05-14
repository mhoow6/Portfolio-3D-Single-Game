using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDead : BossMonsterAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().Dead();
    }
}
