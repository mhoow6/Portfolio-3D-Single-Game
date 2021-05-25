using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Load From Table
    public byte level;
    public float hp;
    public float mp;
    public float sp;
    public float attack_sp;
    public float combat_attack_sp;
    public float combat_skill_01_mp;
    public float combat_skill_01_sp;
    public float combat_skill_01_cooldown;
    public float combat_skill_02_mp;
    public float combat_skill_02_sp;
    public float combat_skill_02_cooldown;
    public float run_speed;
    public float run_sp;
    public float combat_walk_speed;
    public float roll_distance;
    public float roll_sp;
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;
    // Load From Table

    public float currentHp;
    public float currentMp;
    public float currentSp;
    public float SpRecoveryPoint;
    public float runningSpReductionRate;
    public float current_combat_skill_01_cooldown;
    public float current_combat_skill_02_cooldown;
    public bool isPlayerNeedSP;
    public bool isCombatMode;
    public bool isPlayerWalk;
    public bool isPlayerUseCombatSkill01;
    public bool isPlayerUseCombatSkill02;
    
    private Transform sheath;
    private Transform righthand;
    private GameObject item_SwordSheath;
    private GameObject item_Sword;
    private float SpRecoveryDuration;
    private float SkillDuration;
    private bool SpRecovery_running;
    private bool CombatSkill01Cooldown_running;
    private bool CombatSkill02Cooldown_running;
    
    void Start()
    {
        level = PlayerInfoTableManager.playerInfo.level;
        hp = PlayerInfoTableManager.playerInfo.hp;
        mp = PlayerInfoTableManager.playerInfo.mp;
        sp = PlayerInfoTableManager.playerInfo.sp;
        attack_sp = PlayerInfoTableManager.playerInfo.attack_sp;
        combat_attack_sp = PlayerInfoTableManager.playerInfo.combat_attack_sp;
        combat_skill_01_mp = PlayerInfoTableManager.playerInfo.skill_01_mp;
        combat_skill_01_sp = PlayerInfoTableManager.playerInfo.skill_01_sp;
        combat_skill_01_cooldown = PlayerInfoTableManager.playerInfo.skill_01_cooldown;
        combat_skill_02_mp = PlayerInfoTableManager.playerInfo.skill_02_mp;
        combat_skill_02_sp = PlayerInfoTableManager.playerInfo.skill_02_sp;
        combat_skill_02_cooldown = PlayerInfoTableManager.playerInfo.skill_02_cooldown;
        walk_speed = PlayerInfoTableManager.playerInfo.walk_speed;
        run_speed = PlayerInfoTableManager.playerInfo.run_speed;
        run_sp = PlayerInfoTableManager.playerInfo.run_sp;
        combat_walk_speed = PlayerInfoTableManager.playerInfo.combat_walk_speed;
        roll_distance = PlayerInfoTableManager.playerInfo.roll_distance;
        roll_sp = PlayerInfoTableManager.playerInfo.roll_sp;
        basic_weapon_id = PlayerInfoTableManager.playerInfo.basic_weapon_id;
        equip_weapon_id = PlayerInfoTableManager.playerInfo.equip_weapon_id;
        attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
        attack_damage = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage;
        attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_distance;

        currentHp = hp;
        currentMp = mp;
        currentSp = sp;
        current_combat_skill_01_cooldown = 0;
        current_combat_skill_02_cooldown = 0;

        isCombatMode = false;

        SpRecoveryDuration = 0.75f;
        SkillDuration = 1f;

        SpRecoveryPoint = 3f;
        runningSpReductionRate = 5f;

        isPlayerNeedSP = false;
        SpRecovery_running = false;

        isPlayerUseCombatSkill01 = false;
        isPlayerUseCombatSkill02 = false;
        CombatSkill01Cooldown_running = false;
        CombatSkill02Cooldown_running = false;
           
        sheath = GetSheathParent();
        righthand = GetRighthandParent();
        item_SwordSheath = SetWeaponToSheath(equip_weapon_id);
        item_Sword = SetWeaponToRighthand(equip_weapon_id);
    }

    private void Update()
    {
        if (isPlayerNeedSP && !SpRecovery_running)
            StartCoroutine(SpRecovery(SpRecoveryDuration));

        if (isPlayerUseCombatSkill01 && !CombatSkill01Cooldown_running)
            StartCoroutine(CombatSkill01Cooldown(SkillDuration));

        if (isPlayerUseCombatSkill02 && !CombatSkill02Cooldown_running)
            StartCoroutine(CombatSkill02Cooldown(SkillDuration));
    }

    public void Attack(int ani_id)
    {
        switch(ani_id)
        {
            case (int)PlayerAnimation.AniType.ATTACK_02:
                attack_angle = PlayerInfoTableManager.playerInfo.attack_02_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_02:
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_02_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_03:
                attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_03_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                attack_angle = PlayerInfoTableManager.playerInfo.skill_01_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
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

    // 필요 있을까?
    public void WeaponSwitch()
    {
        if (item_SwordSheath.activeSelf == true)
        {
            attack_damage = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_damage;
            attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
            attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_01_angle;

            item_SwordSheath.SetActive(false);
            item_Sword.SetActive(true);
        }
        else
        {
            attack_damage = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage;
            attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_distance;
            attack_angle = PlayerInfoTableManager.playerInfo.combat_attack_01_angle;

            item_SwordSheath.SetActive(true);
            item_Sword.SetActive(false);
        }

    }

    // 매개변수 문자열, 로직 합체
    private Transform GetSheathParent()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Transform spine;    

        foreach (Transform child in children)
        {
            if (child.name == "Spine_03")
            {
                spine = child;
                return spine;
            }     
        }
        return null;
    }

    private Transform GetRighthandParent()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        Transform hand;

        foreach (Transform child in children)
        {
            if (child.name == "Hand_R")
            {
                hand = child;
                return hand;
            }
        }
        return null;
    }
    // 매개변수 문자열, 로직 합체

    // 무기 스위칭시 부모의 위치를 바꾸게 하는 방법이 낫지 않을까?
    private GameObject SetWeaponToSheath(ushort equipWeaponID)
    {
        GameObject _weapon = Resources.Load<GameObject>("Weapon/" + WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).weapon_name);
        GameObject weapon = Instantiate(_weapon);

        weapon.name = "Weapon (Spine)";
        weapon.transform.SetParent(sheath);
        weapon.transform.localPosition = new Vector3(-17.6000004f, 24.7000008f, 18);
        weapon.transform.localRotation = Quaternion.Euler(new Vector3(288.065979f, 201.414993f, 98.0909805f));

        return weapon;
    }

    private GameObject SetWeaponToRighthand(ushort equipWeaponID)
    {
        GameObject _weapon = Resources.Load<GameObject>("Weapon/" + WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equipWeaponID).weapon_name);
        GameObject weapon = Instantiate(_weapon);

        weapon.name = "Weapon (RH)";
        weapon.transform.SetParent(righthand);
        weapon.transform.localPosition = new Vector3(11.6000004f, 1.89999998f, 0.699999988f);
        weapon.transform.localRotation = Quaternion.Euler(new Vector3(80.5413818f, 22.6469021f, 205.775742f));
        weapon.SetActive(false);

        return weapon;
    }

    private IEnumerator SpRecovery(float recoveryDuration)
    {
        SpRecovery_running = true;
        WaitForSeconds wt = new WaitForSeconds(recoveryDuration);

        while (currentSp < sp)
        {
            yield return wt;

            if (isPlayerNeedSP)
                currentSp += SpRecoveryPoint;

            if (currentSp >= sp)
            {
                currentSp = sp;
                isPlayerNeedSP = false;
                SpRecovery_running = false;
                yield break;
            }
        }
    }

    // 매개변수를 통해 다르게 하자
    private IEnumerator CombatSkill01Cooldown(float cooldownDuration)
    {
        CombatSkill01Cooldown_running = true;
        WaitForSeconds wt = new WaitForSeconds(cooldownDuration);

        while (current_combat_skill_01_cooldown != 0)
        {
            yield return wt;
            current_combat_skill_01_cooldown -= cooldownDuration;

            if (current_combat_skill_01_cooldown == 0)
            {
                CombatSkill01Cooldown_running = false;
                yield break;
            }

        }
    }

    private IEnumerator CombatSkill02Cooldown(float cooldownDuration)
    {
        CombatSkill02Cooldown_running = true;
        WaitForSeconds wt = new WaitForSeconds(cooldownDuration);

        while (current_combat_skill_02_cooldown != 0)
        {
            yield return wt;
            current_combat_skill_02_cooldown -= cooldownDuration;

            if (current_combat_skill_02_cooldown == 0)
            {
                CombatSkill02Cooldown_running = false;
                yield break;
            }

        }
    }
}
