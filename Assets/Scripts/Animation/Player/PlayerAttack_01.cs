using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_01 : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.attack_sp;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 이전 애니메이션 -> 현재 애니메이션으로 바뀌면서 현재 애니메이션 도중 이전 애니메이션 스크립트에 의해 boolean값이 바뀌게 되는 경우가 존재.
        // 일일이 애니메이션 트랜지션 시간계산을 하기엔 번거로우므로 차라리 Update 도중 true로 고정시키기로 결정.
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        if (currentAnimationTime > interruptAvailableTime)
        {
            Attack_02_Condition(animator);
            RollCondition(animator);
            return; // 함수 종료로 인해 다른 애니메이션 조건을 무시하고 해당 조건에 맞는 애나메이션 바로 진입
        }

        IdleCondition(animator);
        WalkCondition(animator);
        RunCondition(animator);
        DeadCondition(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        GameManager.instance.controller.immobile = false;
        currentAnimationTime = 0;
    }
}
