using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Runtime.InteropServices;

public class Player : Character
{
    public byte level;
    public float hp;
    public float mp;
    public float sp;
    public float currentHp;
    public float currentMp;
    public float currentSp;
    public float currentExp;
    public float run_speed;
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;
    public float current_combat_skill_01_cooldown;
    public float current_combat_skill_02_cooldown;
    public bool isPlayerNeedSP;
    public bool isCombatMode;
    public bool isPlayerUseCombatSkill01;
    public bool isPlayerUseCombatSkill02;
    public float SkillDuration = 1f;
    public GameObject weapon;
    public NPC boundCollideNPC;
    public bool isBoundCollide;
    public GameObject dialogIcon;
    public float basic_damage;

    private bool isWeaponInRHand;
    private Transform sheath;
    private Transform righthand;
    private Vector3 weaponSheathLocalPos = new Vector3(-17.6000004f, 24.7000008f, 18);
    private Quaternion weaponSheathLocalRot = Quaternion.Euler(new Vector3(288.065979f, 201.414993f, 98.0909805f));
    private Vector3 weaponRHandLocalPos = new Vector3(11.6000004f, 1.89999998f, 0.699999988f);
    private Quaternion weaponRHandLocalRot = Quaternion.Euler(new Vector3(80.5413818f, 22.6469021f, 205.775742f));
    private const float SpRecoveryDuration = 0.5f;
    private const float REQUIRED_MOB_ENDURANCE_BREAK = 0.03F;
    private const float BOUND_COLLIDED_DETECT_DURATION = 0.25F;
    private Vector3 dialogIconLocalPos = new Vector3(0.336f, 1.766f, 0);

    private PlayerAttackEffect attackEffect;
    private PlayerSkillBackEffect skillBackEffect;
    private PlayerHitEffect attackHitEffect;
    public PlayerFootstepEffect footStepEffect;

    // For KnockBack
    public NavMeshAgent agent;
    private float MAX_HIT_HEIGHT = 0.4f;
    private float JUMP_SENSIVITY = 0.3f;
    private bool isJump;

    float normalizeBackDistance;
    float timer = 0f;
    Vector3 origin = Vector3.zero;
    Vector3 goal = Vector3.zero;

    private void Awake()
    {
        current_combat_skill_01_cooldown = 0;
        current_combat_skill_02_cooldown = 0;
        isCombatMode = false;
        isPlayerNeedSP = false;
        isPlayerUseCombatSkill01 = false;
        isPlayerUseCombatSkill02 = false;
        isWeaponInRHand = false;

        dialogIcon = new GameObject("Dialog Icon");
        dialogIcon.transform.SetParent(this.transform);
        dialogIcon.transform.localPosition = dialogIconLocalPos;
    }

    void Start()
    {
        if (!SceneInfoManager.instance.isTempDataExists)
        {
            level = PlayerInfoTableManager.playerInfo.level;
            currentHp = PlayerInfoTableManager.playerInfo.hp;
            currentMp = PlayerInfoTableManager.playerInfo.mp;
            currentSp = PlayerInfoTableManager.playerInfo.sp;
            currentExp = PlayerInfoTableManager.playerInfo.exp;
        }

        if (SceneInfoManager.instance.isTempDataExists)
        {
            level = PlayerInfoTableManager.playerTempInfo.level;
            currentHp = PlayerInfoTableManager.playerTempInfo.currentHp;
            currentMp = PlayerInfoTableManager.playerTempInfo.currentMp;
            currentSp = PlayerInfoTableManager.playerTempInfo.currentSp;
            currentExp = PlayerInfoTableManager.playerTempInfo.currentExp;
        }

        hp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(level).hp;
        mp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(level).mp;
        sp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(level).sp;

        if (currentSp <= sp)
            isPlayerNeedSP = true;

        equip_weapon_id = PlayerInfoTableManager.playerEquipment[(int)EquipmentIndex.WEAPON].id;
        walk_speed = PlayerInfoTableManager.playerInfo.walk_speed;
        run_speed = PlayerInfoTableManager.playerInfo.run_speed;
        basic_weapon_id = PlayerInfoTableManager.playerInfo.basic_weapon_id;
        attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
        attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage, level);
        basic_damage = attack_damage;
        attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_distance;

        sheath = GetNodeObject("Spine_03");
        righthand = GetNodeObject("Hand_R");
        weapon = GetWeaponFromResource(equip_weapon_id);

        StartCoroutine(SpRecovery(SpRecoveryDuration));
        StartCoroutine(CombatSkill01Cooldown(SkillDuration * Time.deltaTime));
        StartCoroutine(CombatSkill02Cooldown(SkillDuration * Time.deltaTime));
        StartCoroutine(FindBoundCollideNPC(BOUND_COLLIDED_DETECT_DURATION));

        // Get Bound
        bound = GetBoundFromSkinnedMeshRenderer(this).Value;
    }

    private void Update()
    {
        BoundUpdate(false);
    }

    public void Attack(int ani_id)
    {
        switch(ani_id)
        {
            case (int)PlayerAnimation.AniType.ATTACK_01:
                attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
                break;

            case (int)PlayerAnimation.AniType.ATTACK_02:
                attack_angle = PlayerInfoTableManager.playerInfo.attack_02_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
                attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
                attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_02_angle;
                attackEffect = EffectManager.instance.CreateAttackEffect((int)PlayerAnimation.AniType.COMBAT_ATTACK_01);
                attackEffect.self.Play();
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_02:
                attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
                attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_02_angle;
                attackEffect = EffectManager.instance.CreateAttackEffect((int)PlayerAnimation.AniType.COMBAT_ATTACK_02);
                attackEffect.self.Play();
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_03:
                attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
                attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_03_angle;
                attackEffect = EffectManager.instance.CreateAttackEffect((int)PlayerAnimation.AniType.COMBAT_ATTACK_03);
                attackEffect.self.Play();
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                attack_damage = IncreaseDamageByLevel(PlayerInfoTableManager.playerInfo.skill_01_damage, level);
                attack_distance = PlayerInfoTableManager.playerInfo.skill_01_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.skill_01_angle;
                attackEffect = EffectManager.instance.CreateAttackEffect((int)PlayerAnimation.AniType.COMBAT_SKILL_01);
                skillBackEffect.StopEffect();
                attackEffect.self.Play();
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                attack_damage = IncreaseDamageByLevel(PlayerInfoTableManager.playerInfo.skill_02_damage, level);
                attack_distance = PlayerInfoTableManager.playerInfo.skill_02_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.skill_02_angle;
                attackEffect = EffectManager.instance.CreateAttackEffect((int)PlayerAnimation.AniType.COMBAT_SKILL_02);
                skillBackEffect.StopEffect();
                attackEffect.self.Play();
                break;
        }


        foreach (Monster mob in GameManager.instance.monsters)
        {
            float PlayerAndMonster_Distance = Vector3.Distance(transform.position, mob.transform.position);
            float PlayerAndMonster_Angle = Mathf.Acos(Vector3.Dot
                (transform.forward,
                (mob.transform.position - transform.position).normalized)
                ) * Mathf.Rad2Deg;
            float RequiredToIncreaseStackDamage = MonsterInfoTableManager.GetMonsterOriginHpFromID(mob.id) * REQUIRED_MOB_ENDURANCE_BREAK;

            if (mob != null &&
                mob.gameObject.activeSelf &&
                mob.hp > 0 &&
                PlayerAndMonster_Distance <= attack_distance &&
                PlayerAndMonster_Angle <= attack_angle &&
                !mob.isImmortal)
            {
                mob.hp -= attack_damage;

                if (!GameManager.instance.controller.cameraArm.isJitter)
                    StartCoroutine(GameManager.instance.controller.cameraArm.jitterCamera(attack_damage));

                AudioManager.instance.PlayAudioAtPoint(AudioManager.instance.GetAudio(AudioCondition.ALL, AudioCondition.MONSTER_HIT), transform.position, AudioManager.instance._HIT_SOUND);

                attackHitEffect = EffectManager.instance.CreateHitEffect(ani_id, mob);

                if (attackHitEffect != null)
                    attackHitEffect.PlayEffects(mob);

                if (!mob.isStuned)
                    mob.endurance_stack += EnduranceStackCalculator(attack_damage, RequiredToIncreaseStackDamage);
                    
            }
        }
    }

    public void WeaponSwitch()
    {
        if (isWeaponInRHand == false)
        {
            attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
            attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
            attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_01_angle;
            basic_damage = attack_damage;

            weapon.transform.SetParent(righthand);
            weapon.transform.localPosition = weaponRHandLocalPos;
            weapon.transform.localRotation = weaponRHandLocalRot;
            isWeaponInRHand = true;
        }
        else
        {
            attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage, level);
            attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_distance;
            attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_01_angle;
            basic_damage = attack_damage;

            weapon.transform.SetParent(sheath);
            weapon.transform.localPosition = weaponSheathLocalPos;
            weapon.transform.localRotation = weaponSheathLocalRot;
            isWeaponInRHand = false;
        }
    }

    public void Dead()
    {
        HUDManager.instance.dead.gameObject.SetActive(true);
    }

    public GameObject GetWeaponFromResource(ushort equipWeaponID)
    {
        GameObject _weapon = ResourceManager.weapon.LoadAsset<GameObject>(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).prefab_name);
        GameObject weapon = Instantiate(_weapon);

        weapon.name = "Weapon";

        if (isWeaponInRHand == false)
        {
            weapon.transform.SetParent(sheath);
            weapon.transform.localPosition = weaponSheathLocalPos;
            weapon.transform.localRotation = weaponSheathLocalRot;
        }
        else if (isWeaponInRHand == true)
        {
            attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).basic_damage, level);
            attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).basic_distance;
            attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_01_angle;
            basic_damage = attack_damage;

            weapon.transform.SetParent(righthand);
            weapon.transform.localPosition = weaponRHandLocalPos;
            weapon.transform.localRotation = weaponRHandLocalRot;
        }

        return weapon;
    }

    public void LevelUpCheck()
    {
        if (GameManager.instance.controller.player.currentExp >= PlayerExpInfoTableManager.GetPlayerExpInfoFromLevel(GameManager.instance.controller.player.level).max_exp)
        {
            GameManager.instance.controller.player.level++;

            GameManager.instance.controller.player.hp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).hp;
            GameManager.instance.controller.player.mp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).mp;
            GameManager.instance.controller.player.sp = PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).sp;

            LevelUpCheck();
            HUDManager.instance.levelup.gameObject.SetActive(true);
        }
    }

    private Transform GetNodeObject(string nodeName)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Transform hand;

        foreach (Transform child in children)
        {
            if (child.name == nodeName)
            {
                hand = child;
                return hand;
            }
        }
        return null;
    }

    private IEnumerator SpRecovery(float recoveryDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(recoveryDuration);

        while (true)
        {
            yield return wt;

            if (isPlayerNeedSP)
                currentSp += PlayerInfoTableManager.playerInfo.sp_recovery_point;

            if (currentSp >= sp)
            {
                currentSp = sp;
                isPlayerNeedSP = false;
            }
        }
    }

    private IEnumerator CombatSkill01Cooldown(float cooldownDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(cooldownDuration);

        while (true)
        {
            yield return wt;

            if (isPlayerUseCombatSkill01)
                current_combat_skill_01_cooldown -= cooldownDuration;

            if (current_combat_skill_01_cooldown < 0)
                current_combat_skill_01_cooldown = 0;
        }
    }

    private IEnumerator CombatSkill02Cooldown(float cooldownDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(cooldownDuration);

        while (true)
        {
            yield return wt;

            if (isPlayerUseCombatSkill02)
                current_combat_skill_02_cooldown -= cooldownDuration;

            if (current_combat_skill_02_cooldown < 0)
                current_combat_skill_02_cooldown = 0;
        }
    }

    private float IncreaseDamageByLevel(float currentDamage, byte level)
    {
        return currentDamage + (level*(level+1));
    }

    private byte EnduranceStackCalculator(float currentDamage, float RequiredToIncreaseStackDamage)
    {
        return (byte)(currentDamage / RequiredToIncreaseStackDamage);
    }

    private IEnumerator FindBoundCollideNPC(float detectDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(detectDuration);

        yield return null;

        while (true)
        {
            if (GameManager.instance.npcs.Count != 0)
            {
                Character collided = GameManager.instance.npcs.Find(npc => this.bound.Intersects(npc._bound));

                if (collided != null)
                    OnBoundEnter(collided);
                else
                    OnBoundEscape();
            }

            yield return wt;
        }
    }

    protected override void OnBoundEnter(Character collided)
    {
        isBoundCollide = true;
        boundCollideNPC = (NPC)collided;
        HUDManager.instance.inGame.icons.dialogIcon.gameObject.SetActive(true);
    }

    protected override void OnBoundEscape()
    {
        isBoundCollide = false;
        boundCollideNPC = null;
        HUDManager.instance.inGame.icons.dialogIcon.gameObject.SetActive(false);
    }

    public void BackEffect(int ani_id)
    {
        switch (ani_id)
        {
            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                skillBackEffect = EffectManager.instance.CreateESkillBackEffect();
                skillBackEffect.PlayEffect();
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                skillBackEffect = EffectManager.instance.CreateQSkillBackEffect();
                skillBackEffect.PlayEffect();
                break;
        }
    }

    public void KnockBackEffect(float damage)
    {
        StartCoroutine(GameManager.instance.controller.cameraArm.jitterCamera(damage));
        StartCoroutine(JumpingKnockBack(damage));
    }

    private IEnumerator JumpingKnockBack(float damage)
    {
        if (!isJump)
        {
            normalizeBackDistance = damage * 0.001f;
            normalizeBackDistance = normalizeBackDistance > 0.3f ? 0.3f : normalizeBackDistance;

            isJump = true;
            agent.enabled = false;
            timer = 0;

            origin = transform.position;
            goal = -transform.forward + new Vector3(transform.position.x, transform.position.y + MAX_HIT_HEIGHT, transform.position.z);

            while (transform.position.y != goal.y)
            {
                timer += Time.deltaTime * JUMP_SENSIVITY;
                transform.position = Vector3.MoveTowards(transform.position, goal, timer);
                yield return null;
            }

            timer = 0f;
            goal = -transform.forward * normalizeBackDistance + new Vector3(transform.position.x, origin.y, transform.position.z);

            while (transform.position.y != origin.y)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, goal, timer);
                yield return null;
            }

            isJump = false;
            agent.enabled = true;
            timer = 0;
        }
    }
}
