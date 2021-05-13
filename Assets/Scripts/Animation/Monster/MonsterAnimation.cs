using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MonsterAnimation : StateMachineBehaviour
{
    protected float prevHP;

    abstract protected void SwitchIdle(Animator animator);
    abstract protected void SwitchWalk(Animator animator);
    abstract protected void SwitchRun(Animator animator);
    abstract protected void SwitchInjured(Animator animator, float prevHP);
    abstract protected void SwitchDead(Animator animator);
    abstract protected void SwitchAttack(Animator animator);

}
