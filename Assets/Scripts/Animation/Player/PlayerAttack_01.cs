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
        // ���� �ִϸ��̼� -> ���� �ִϸ��̼����� �ٲ�鼭 ���� �ִϸ��̼� ���� ���� �ִϸ��̼� ��ũ��Ʈ�� ���� boolean���� �ٲ�� �Ǵ� ��찡 ����.
        // ������ �ִϸ��̼� Ʈ������ �ð������ �ϱ⿣ ���ŷο�Ƿ� ���� Update ���� true�� ������Ű��� ����.
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        if (currentAnimationTime > interruptAvailableTime)
        {
            Attack_02_Condition(animator);
            RollCondition(animator);
            return; // �Լ� ����� ���� �ٸ� �ִϸ��̼� ������ �����ϰ� �ش� ���ǿ� �´� �ֳ����̼� �ٷ� ����
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
