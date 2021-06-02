using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterDead : EliteMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<EliteMonster>();
        self.Dead();
        self.hp = 0;
        self.endurance_stack = 0;
    }
}
