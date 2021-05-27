using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_01 : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.attack_sp;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� �ִϸ��̼� -> ���� �ִϸ��̼����� �ٲ�鼭 ���� �ִϸ��̼� ���� ���� �ִϸ��̼� ��ũ��Ʈ�� ���� boolean���� �ٲ�� �Ǵ� ��찡 ����.
        // ������ �ִϸ��̼� Ʈ������ �ð������ �ϱ⿣ ���ŷο�Ƿ� ���� Update ���� true�� ������Ű��� ����.
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        // ��ȯ�ð� ������ �ִϸ��̼� ���� �ð� ��ȭ. �ִϸ��̼� ���� �ð��� Ŭ���� speed�� �����
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * attackClipSpeed;

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
