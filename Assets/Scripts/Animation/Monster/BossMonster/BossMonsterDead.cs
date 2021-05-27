using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterDead : BossMonsterAnimation
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<BossMonster>();
        self.Dead();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.hp = 0;
    }
}
