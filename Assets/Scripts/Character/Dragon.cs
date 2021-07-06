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
    public float _flyAttackDistance { get => skill_3_distance; }

    private Transform fireBallEffectPos;
    public Transform _fireBallEffectPos { get => fireBallEffectPos; }

    public float _specialComboCooldown { get => specialComboCooldown; }
    public float specialComboCooldown;
    public bool isSpecialComboCooldown;

    public float _originSpeed { get => walk_speed; }
    public float _flyingSpeed { get => run_speed; }

    public bool isPlayerEncounted;

    private Vector3 vToPlayer;
    private Vector3 vToLeft;
    private float angleWithLeft;
    private Vector3 vTurnAround;

    private const float FIREBALL_DURATION = 10f;
    private const float FIREBALL_AFTERFREEZE = 2f;
    private const float SPECAIL_COMBO_COOLDOWN = 120f;

    private DragonFireBallEffect fireBallEffect;

    public int _TOTAL_FIYBALL_SHOOT { get => TOTAL_FIYBALL_SHOOT; }
    private const int TOTAL_FIYBALL_SHOOT = 5;
    public int currentFlyFireBallShoot;
    private const float FLY_HEIGHT = 2.0f;
    public float _SPARE_DISTANCE { get => SPARE_DISTANCE; }
    private const float SPARE_DISTANCE = 1f;
    public float _ATTACK_MIN_ANGLE { get => ATTACK_MIN_ANGLE; }
    private const float ATTACK_MIN_ANGLE = 20f;
    public float _FIREBALL_MIN_ANGLE { get => FIREBALL_MIN_ANGLE; }
    private const float FIREBALL_MIN_ANGLE = 5f;
    public float _TURN_AROUND_SPEED { get => TURN_AROUND_SPEED * Time.deltaTime; }
    private const float TURN_AROUND_SPEED = 2f;

    public Vector3 destination;

    private void Start()
    {
        Setup();
        GetNodeObject(this.transform, "Fireball_Effect", ref fireBallEffectPos);

        GameManager.instance.bossCam.boss = this;
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

    public bool IsSpecialComboAble()
    {
        if (hp < spawnInfo.hp * 0.5f)
            if (!isSpecialComboCooldown)
                return true;

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

    public IEnumerator SpecialComboCooldown()
    {
        while (specialComboCooldown <= SPECAIL_COMBO_COOLDOWN)
        {
            isSpecialComboCooldown = true;
            yield return null;
            specialComboCooldown += Time.deltaTime;
        }

        isSpecialComboCooldown = false;
        specialComboCooldown = 0;
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
            case (int)DragonAnimation.ComboAniType.FLY_FIREBALL:
                currentAttackDamage = skill_2_damage;
                fireBallEffect = EffectManager.instance.CreateDragonFireBallEffect(this);
                fireBallEffect.currentDamage = currentAttackDamage;
                fireBallEffect.PlayEffect(ani_id, this);
                break;
            case (int)DragonAnimation.ComboAniType.METEO:
                currentAttackDamage = skill_3_damage;
                fireBallEffect = EffectManager.instance.CreateDragonMeteoEffect(this);
                fireBallEffect.currentDamage = currentAttackDamage;
                fireBallEffect.PlayEffect(ani_id, this);
                break;
        }
    }

    public Vector3 SetLandingPos()
    {
        float distancePlayerFromFarField = Vector3.Distance(GameManager.instance.controller.player.transform.position, GameManager.instance.dragonLandFields[0].position);
        Vector3 landingPos = GameManager.instance.dragonLandFields[0].position;

        for (int i = 0; i < GameManager.instance.dragonLandFields.Count; i++)
        {
            float calculatedDistance = Vector3.Distance(GameManager.instance.controller.player.transform.position, GameManager.instance.dragonLandFields[i].position);

            if (calculatedDistance > distancePlayerFromFarField)
            {
                distancePlayerFromFarField = calculatedDistance;
                landingPos = GameManager.instance.dragonLandFields[i].position;
            }
        }

        return landingPos;
    }

    public Vector3 SetFlyingMovePos(Vector3 destination)
    {
        Vector3 reCaculatedPos = new Vector3(destination.x, transform.position.y, destination.z);

        return reCaculatedPos;
    }

    public IEnumerator Flying()
    {
        Vector3 des = new Vector3(transform.position.x, transform.position.y + FLY_HEIGHT, transform.position.z);
        float timer = 0;

        while (transform.position.y < des.y)
        {
            Debug.Log("current y: " + transform.position.y); // 이거 왜 날기 시작 전 위치 값밖에 안 나옴?
            timer += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, des, timer);
            yield return null;
        }

        Debug.Log("더 이상 안 날래");
    }
}
