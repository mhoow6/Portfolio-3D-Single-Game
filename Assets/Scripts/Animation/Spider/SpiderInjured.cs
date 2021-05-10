using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderInjured : SpiderAnimation
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Monster>().HP <= 0)
            animator.SetInteger("ani_id", (int)AniType.DEAD);

        animator.SetInteger("ani_id", (int)AniType.IDLE);
    }
}
