using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIdle : SpiderAnimation
{
    private int prevHP;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        prevHP = animator.GetComponent<Monster>().HP;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Monster>().HP < prevHP)
            animator.SetInteger("ani_id", (int)AniType.INJURED);

        if (animator.GetComponent<Monster>().HP <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);
    }

}
