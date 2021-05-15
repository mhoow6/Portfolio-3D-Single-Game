using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public byte level;
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;
    public float combat_walk_speed;

    // 추후에 자동 로드
    public GameObject item_SwordSheath;
    public GameObject item_Sword;

    void Awake()
    {
        level = PlayerInfoTableManager.playerInfo.level;
        hp = PlayerInfoTableManager.playerInfo.hp;
        walk_speed = PlayerInfoTableManager.playerInfo.walk_speed;
        combat_walk_speed = PlayerInfoTableManager.playerInfo.combat_walk_speed;
        basic_weapon_id = PlayerInfoTableManager.playerInfo.basic_weapon_id;
        equip_weapon_id = PlayerInfoTableManager.playerInfo.equip_weapon_id; // 장비창 구현시 선택한 장비의 아이디를 적용하게 끔 구현
        attack_angle = PlayerInfoTableManager.playerInfo.attack_01_angle;
        attack_damage = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(basic_weapon_id).basic_damage;
        attack_distance = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(equip_weapon_id).basic_distance;
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

            if (mob.gameObject.activeSelf == true &&
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
}
