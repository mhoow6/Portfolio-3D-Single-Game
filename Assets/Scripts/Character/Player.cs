using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            hp = PlayerInfoTableManager.playerInfo.hp;
            mp = PlayerInfoTableManager.playerInfo.mp;
            sp = PlayerInfoTableManager.playerInfo.sp;
            currentHp = PlayerInfoTableManager.playerInfo.hp;
            currentMp = PlayerInfoTableManager.playerInfo.mp;
            currentSp = PlayerInfoTableManager.playerInfo.sp;
        }
        else
        {
            level = PlayerInfoTableManager.playerTempInfo.level;
            hp = PlayerInfoTableManager.playerTempInfo.hp;
            mp = PlayerInfoTableManager.playerTempInfo.mp;
            sp = PlayerInfoTableManager.playerTempInfo.sp;
            currentHp = PlayerInfoTableManager.playerTempInfo.currentHp;
            currentMp = PlayerInfoTableManager.playerTempInfo.currentMp;
            currentSp = PlayerInfoTableManager.playerTempInfo.currentSp;
        }

        if (currentSp <= sp)
            isPlayerNeedSP = true;

        equip_weapon_id = PlayerEquipmentTableManager.playerEquipment[(int)EquipmentIndex.WEAPON].id;
        walk_speed = PlayerInfoTableManager.playerInfo.walk_speed;
        run_speed = PlayerInfoTableManager.playerInfo.run_speed;
        basic_weapon_id = PlayerInfoTableManager.playerInfo.basic_weapon_id;
        attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
        attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage, level);
        attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_distance;

        sheath = GetNodeObject("Spine_03");
        righthand = GetNodeObject("Hand_R");
        weapon = GetWeaponFromResource(equip_weapon_id);

        StartCoroutine(SpRecovery(SpRecoveryDuration));
        StartCoroutine(CombatSkill01Cooldown(SkillDuration * Time.deltaTime));
        StartCoroutine(CombatSkill02Cooldown(SkillDuration * Time.deltaTime));
        StartCoroutine(FindBoundCollideChar(BOUND_COLLIDED_DETECT_DURATION));

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
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_02:
                attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
                attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_02_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_03:
                attack_damage = IncreaseDamageByLevel(WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage, level);
                attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_03_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                attack_damage = IncreaseDamageByLevel(PlayerInfoTableManager.playerInfo.skill_01_damage, level);
                attack_distance = PlayerInfoTableManager.playerInfo.skill_01_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.skill_01_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                attack_damage = IncreaseDamageByLevel(PlayerInfoTableManager.playerInfo.skill_02_damage, level);
                attack_distance = PlayerInfoTableManager.playerInfo.skill_02_distance;
                attack_angle = PlayerInfoTableManager.playerInfo.skill_02_angle;
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
                mob.gameObject.activeSelf == true &&
                PlayerAndMonster_Distance <= attack_distance &&
                PlayerAndMonster_Angle <= attack_angle)
            {
                mob.hp -= attack_damage;

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

            weapon.transform.SetParent(sheath);
            weapon.transform.localPosition = weaponSheathLocalPos;
            weapon.transform.localRotation = weaponSheathLocalRot;

            isWeaponInRHand = false;
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

    public GameObject GetWeaponFromResource(ushort equipWeaponID)
    {
        GameObject _weapon = Resources.Load<GameObject>("Weapon/" + WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).prefab_name);
        GameObject weapon = Instantiate(_weapon);

        weapon.name = "Weapon";
        weapon.transform.SetParent(sheath);
        weapon.transform.localPosition = weaponSheathLocalPos;
        weapon.transform.localRotation = weaponSheathLocalRot;

        return weapon;
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

    private IEnumerator FindBoundCollideChar(float detectDuration)
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
}
