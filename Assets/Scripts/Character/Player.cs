using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isPlayerWalk;
    public bool isPlayerUseCombatSkill01;
    public bool isPlayerUseCombatSkill02;
    public float SkillDuration = 1f;

    private GameObject weapon;
    private Transform sheath;
    private Transform righthand;
    private Vector3 weaponSheathLocalPos = new Vector3(-17.6000004f, 24.7000008f, 18);
    private Quaternion weaponSheathLocalRot = Quaternion.Euler(new Vector3(288.065979f, 201.414993f, 98.0909805f));
    private Vector3 weaponRHandLocalPos = new Vector3(11.6000004f, 1.89999998f, 0.699999988f);
    private Quaternion weaponRHandLocalRot = Quaternion.Euler(new Vector3(80.5413818f, 22.6469021f, 205.775742f));
    private const float SpRecoveryDuration = 0.5f;
    private bool isWeaponInRHand;

    private void Awake()
    {
        current_combat_skill_01_cooldown = 0;
        current_combat_skill_02_cooldown = 0;
        isCombatMode = false;
        isPlayerNeedSP = false;
        isPlayerUseCombatSkill01 = false;
        isPlayerUseCombatSkill02 = false;
        isWeaponInRHand = false;
    }

    void Start()
    {
        level = PlayerInfoTableManager.playerInfo.level;
        hp = PlayerInfoTableManager.playerInfo.hp;
        mp = PlayerInfoTableManager.playerInfo.mp;
        sp = PlayerInfoTableManager.playerInfo.sp;
        currentHp = PlayerInfoTableManager.playerInfo.hp;
        currentMp = PlayerInfoTableManager.playerInfo.mp;
        currentSp = PlayerInfoTableManager.playerInfo.sp;
        equip_weapon_id = PlayerInfoTableManager.playerInfo.equip_weapon_id;
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
        StartCoroutine(CombatSkill01Cooldown(SkillDuration));
        StartCoroutine(CombatSkill02Cooldown(SkillDuration));
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

            if (mob != null &&
                mob.gameObject.activeSelf == true &&
                PlayerAndMonster_Distance <= attack_distance &&
                PlayerAndMonster_Angle <= attack_angle)
            {
                mob.hp -= attack_damage;
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

    private GameObject GetWeaponFromResource(ushort equipWeaponID)
    {
        GameObject _weapon = Resources.Load<GameObject>("Weapon/" + WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).weapon_name);
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
}
