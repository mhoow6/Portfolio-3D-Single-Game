using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAnimation : StateMachineBehaviour
{
    protected Monster self;
    protected float prevHP;

    protected abstract void IdleCondition(Animator animator, Monster monster);

    protected abstract void WalkCondition(Animator animator, Monster monster);

    protected abstract void RunCondition(Animator animator, Monster monster);

    protected abstract void InjuredCondition(Animator animator, Monster monster, float prevHP);

    protected abstract void DeadCondition(Animator animator, Monster monster);

    protected abstract void AttackCondition(Animator animator, Monster monster);
}
