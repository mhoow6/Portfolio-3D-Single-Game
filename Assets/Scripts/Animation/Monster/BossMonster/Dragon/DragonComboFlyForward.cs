using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonComboFlyForward : DragonAnimation
{
    protected override void StateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aniHandler = KeepFlyCondition;
        aniHandler += LandReadyCondition;
        self.destination = self.SetLandingPos();
        self.agent.destination = self.SetFlyingMovePos(self.destination);
        self.agent.speed = self._flyingSpeed;
    }

    protected override void StateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self.agent.speed = self._originSpeed;
    }
}
