using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimation : StateMachineBehaviour
{
    protected enum AniType
    {
        IDLE = 0,
        WALK,
        RUN,
        INJURED,
        DEAD,
        ATTACK
    }
}
