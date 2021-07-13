using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonMonsterDead : CommonMonsterAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.Dead();
        self.hp = 0;
        self.endurance_stack = 0;
        
        if (self._stunEffect != null)
            self._stunEffect.self.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        animationHandler = null;
        damagedStateHandler = null;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.hp = 0;
        self.endurance_stack = 0;
    }
}
