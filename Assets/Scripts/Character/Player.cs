using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public byte level;
    public float currentHp;
    public float mp;
    public float currentMp;
    public float sp;
    public float currentSp;
    public float attack_sp;
    public float combat_attack_sp;
    public float skill_01_mp;
    public float skill_01_sp;
    public float skill_01_cooldown;
    public float skill_02_mp;
    public float skill_02_sp;
    public float skill_02_cooldown;
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;
    public float combat_walk_speed;
    public Transform sheath;
    public Transform righthand;
    public GameObject item_SwordSheath;
    public GameObject item_Sword;
    public bool isPlayerNeedSP;

    private bool CR_running;

    void Awake()
    {
        level = PlayerInfoTableManager.playerInfo.level;
        hp = PlayerInfoTableManager.playerInfo.hp;
        currentHp = hp;
        mp = PlayerInfoTableManager.playerInfo.mp;
        currentMp = mp;
        sp = PlayerInfoTableManager.playerInfo.sp;
        currentSp = sp;
        attack_sp = PlayerInfoTableManager.playerInfo.attack_sp;
        combat_attack_sp = PlayerInfoTableManager.playerInfo.combat_attack_sp;
        skill_01_mp = PlayerInfoTableManager.playerInfo.skill_01_mp;
        skill_01_sp = PlayerInfoTableManager.playerInfo.skill_01_sp;
        skill_01_cooldown = PlayerInfoTableManager.playerInfo.skill_01_cooldown;
        skill_02_mp = PlayerInfoTableManager.playerInfo.skill_02_mp;
        skill_02_sp = PlayerInfoTableManager.playerInfo.skill_02_sp;
        skill_02_cooldown = PlayerInfoTableManager.playerInfo.skill_02_cooldown;
        walk_speed = PlayerInfoTableManager.playerInfo.walk_speed;
        combat_walk_speed = PlayerInfoTableManager.playerInfo.combat_walk_speed;
        basic_weapon_id = PlayerInfoTableManager.playerInfo.basic_weapon_id;
        equip_weapon_id = PlayerInfoTableManager.playerInfo.equip_weapon_id; // 장비창 구현시 선택한 장비의 아이디를 적용하게 끔 구현
        attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
        attack_damage = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage;
        attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
        sheath = GetSheathParent();
        righthand = GetRighthandParent();
        item_SwordSheath = SetWeaponToSheath(equip_weapon_id);
        item_Sword = SetWeaponToRighthand(equip_weapon_id);
        isPlayerNeedSP = false;
        CR_running = false;
    }

    private void Update()
    {
        GameManager.instance.playerHP.value = currentHp / hp;
        GameManager.instance.playerMP.value = currentMp / mp;
        GameManager.instance.playerSP.value = currentSp / sp;
        GameManager.instance.playerLevel.text = level.ToString();

        if (isPlayerNeedSP && !CR_running)
            StartCoroutine(SpRecovery(1f));
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

            case (int)PlayerAnimation.AniType.SKILL_01:
                attack_angle = PlayerInfoTableManager.playerInfo.skill_01_angle;
                break;

            case (int)PlayerAnimation.AniType.SKILL_02:
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
        CR_running = true;
        WaitForSeconds wt = new WaitForSeconds(recoveryDuration);

        while (currentSp < sp)
        {
            yield return wt;
            currentSp += 3f;

            if (currentSp >= sp)
            {
                currentSp = sp;
                isPlayerNeedSP = false;
                CR_running = false;
                yield break;
            }
        }
    }
}
