using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monster
{
    public float _currentDistanceWithPlayer { get => currentDistanceWithPlayer; }
    public float _currentAngleWithPlayer { get => currentAngleWithPlayer; }

    public float _detectRange { get => detect_range; }

    public float _attackRange { get => attack_distance; }
    public float _attackAngle { get => attack_angle; }

    public float _tailAttackRange { get => skill_1_distance; }

    public float _currentAttackCooldown { get => currentAttackCooldown; }

    private float fireBallCooldown;
    public float _fireBallCooldown { get => fireBallCooldown; }
    public bool isFireBallCooldown;

    private float freezeCooldown;
    public float _freezeCooldown { get => freezeCooldown; }
    public bool isFreeze;

    public float _fireBallDistance { get => skill_2_distance; }

    private Transform fireBallEffectPos;
    public Transform _fireBallEffectPos { get => fireBallEffectPos; }

    private Vector3 vToPlayer;
    private Vector3 vToLeft;
    private float angleWithLeft;
    private Vector3 vTurnAround;

    private const float FIREBALL_DURATION = 10f;
    private const float FIREBALL_AFTERFREEZE = 2f;

    private DragonFireBallEffect fireBalleffect;

    private void Start()
    {
        Setup();
        GetNodeObject(this.transform, "Fireball_Effect", ref fireBallEffectPos);
    }

    private void Update()
    {
        Detector();
    }

    public bool IsTailAttackHitable()
    {
        vToPlayer = (GameManager.instance.controller.player.transform.position - transform.position).normalized;
        vToLeft = -transform.right;
        angleWithLeft = Mathf.Acos(Vector3.Dot(vToLeft, vToPlayer)) * Mathf.Rad2Deg;

        if (float.IsNaN(angleWithLeft)) angleWithLeft = 0f;

        if (angleWithLeft < skill_1_angle && currentAngleWithPlayer > 90f)
            return true;

        return false;
    }

    public bool IsNeedTurnAround()
    {
        if (!IsTailAttackHitable())
        {
            if (currentDistanceWithPlayer < attack_distance)
                if (currentAngleWithPlayer > attack_angle)
                    return true;
        }

        return false;
    }

    public void TurnAround(float turnAroundSpeed)
    {
        vToPlayer = (GameManager.instance.controller.player.transform.position - transform.position).normalized;
        vTurnAround = Vector3.RotateTowards(transform.forward, vToPlayer, turnAroundSpeed, 0.0f);

        transform.rotation = Quaternion.LookRotation(vTurnAround);
    }

    public IEnumerator FireBallCooldown()
    {
        while (fireBallCooldown <= FIREBALL_DURATION)
        {
            isFireBallCooldown = true;
            yield return null;
            fireBallCooldown += Time.deltaTime;
        }

        isFireBallCooldown = false;
        fireBallCooldown = 0;
    }

    public IEnumerator FireBallAfterFreeze()
    {
        while (freezeCooldown <= FIREBALL_AFTERFREEZE)
        {
            isFreeze = true;
            yield return null;
            freezeCooldown += Time.deltaTime;
        }

        isFreeze = false;
        freezeCooldown = 0;
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)DragonAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;

            case (int)DragonAnimation.AniType.TAIL_ATTACK:
                currentAttackDamage = skill_1_damage;
                currentAttackDistance = skill_1_distance;
                currentAttackAngle = skill_1_angle;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle && !GameManager.instance.controller.isPlayerWantToRoll)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
    }

    public void RangeAttack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)DragonAnimation.AniType.FIREBALL:
                currentAttackDamage = skill_2_damage;
                fireBalleffect = EffectManager.instance.CreateDragonFireBallEffect(this);
                fireBalleffect.currentDamage = currentAttackDamage;
                fireBalleffect.PlayEffect(ani_id, this);
                break;
        }
    }
}
