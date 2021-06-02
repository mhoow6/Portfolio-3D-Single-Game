using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterDead : CommonMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<CommonMonster>();
        self.Dead();
        self.hp = 0;
        self.endurance_stack = 0;
    }
}
