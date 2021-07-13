using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBattleTailAttack : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.destination = self.transform.position;
        aniHandler = DeadCondition;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentAnimationTime >= (1 - defaultAnimationTransitionTime))
        {
            // ��ٿ� �÷��״� �ڷ�ƾ���� �ñ�� �� ��. �ڷ�ƾ ����ӵ��� AttackCondition ���Ǻ񱳺��� ������ ������ 2���ϰ� �ȴ�.
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
