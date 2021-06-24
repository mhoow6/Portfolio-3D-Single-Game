using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.Dead();
    }
}
