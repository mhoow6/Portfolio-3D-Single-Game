using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatSkill_02 : PlayerAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.player.currentSp -= PlayerInfoTableManager.playerInfo.skill_02_sp;
        GameManager.instance.controller.player.currentMp -= PlayerInfoTableManager.playerInfo.skill_02_mp;
        GameManager.instance.controller.player.current_combat_skill_02_cooldown = PlayerInfoTableManager.playerInfo.skill_02_cooldown;
        GameManager.instance.controller.player.isPlayerUseCombatSkill02 = true;

        if (GameManager.instance.controller.player.footStepEffect != null)
            GameManager.instance.controller.player.footStepEffect.FootStepChange(AniType.NONE);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = true;
        GameManager.instance.controller.player.isPlayerNeedSP = false;

        CombatModeIdleCondition(animator);
        CombatModeWalkCondition(animator);
        CombatModeAttack_01_Condition(animator);
        CombatModeSkill_01_Condition(animator);
        CombatModeDeadCondition(animator);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.controller.immobile = false;
        GameManager.instance.controller.player.isPlayerNeedSP = true;
    }
}
