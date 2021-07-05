using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboLand : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        specialCombo = false;
        self.isImmortal = false;
        self.isSpecialComboCooldown = true;
        animator.SetBool("specialCombo", false);
        animator.SetInteger("combo_id", (int)ComboAniType.NONE);
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.StartCoroutine(self.SpecialComboCooldown());
    }
}
