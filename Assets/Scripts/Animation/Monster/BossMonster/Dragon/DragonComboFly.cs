using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboFly : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.currentFlyFireBallShoot < self._TOTAL_FIYBALL_SHOOT)
        {
            self.currentFlyFireBallShoot++;
            animator.SetInteger("combo_id", (int)ComboAniType.FLY_FIREBALL);
        }
            
        if (self.currentFlyFireBallShoot >= self._TOTAL_FIYBALL_SHOOT)
            animator.SetInteger("combo_id", (int)ComboAniType.METEO);
    }
}
