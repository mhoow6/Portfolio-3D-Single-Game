using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_01 : PlayerAnimation
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.currentSp -= GameManager.instance.controller.player.attack_sp;
        GameManager.instance.controller.player.isPlayerNeedSP = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {  
        // ��ȯ�ð� ������ �ִϸ��̼� ���� �ð� ��ȭ. �ִϸ��̼� ���� �ð��� Ŭ���� speed�� �����
        if (currentAnimationTime < (1 - animationTransitionTime))
            currentAnimationTime += Time.deltaTime * attackClipSpeed;

        if (currentAnimationTime > attackAvailableTime)
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
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
        currentAnimationTime = 0;
    }
}
