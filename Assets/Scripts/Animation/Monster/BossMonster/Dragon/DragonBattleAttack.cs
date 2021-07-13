using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleAttack : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = DeadCondition;
        self.agent.destination = self.transform.position;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime >= (1 - defaultAnimationTransitionTime))
        {
            // 쿨다운 플래그는 코루틴한테 맡기면 안 됨. 코루틴 실행속도가 AttackCondition 조건비교보다 느려서 공격을 2번하게 된다.
            self.isAttackCooldown = true;
            animator.SetInteger("ani_id", (int)AniType.IDLE);
        }

    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAnimationTime = 0;

        self.StartCoroutine(self.AttackCooldown(self.attack_duration));
    }
}
