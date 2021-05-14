using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;

    // 미구현
    public float combat_move_speed;
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
    public float skill_2_damage;
    public float skill_2_distance;
    public float skill_2_angle;

    // 추후에 자동 로드
    public GameObject item_SwordSheath;
    public GameObject item_Sword;

    void Awake()
    {
        // 추후에 테이블에서 로드하게 하자.
        hp = 100;
        moveSpeed = 3.0f;
        combat_move_speed = 2.0f;
        basic_weapon_id = 3000;
        equip_weapon_id = 3001;
        attack_damage = 20;
        attack_distance = 2f;
        attack_angle = 60f;
    }

    public void Attack(int ani_id)
    {
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

    public void WeaponSwitch()
    {
        if (item_SwordSheath.activeSelf == true)
        {
            attack_damage = ResourceManager.instance.GetWeaponFromWeaponID(equip_weapon_id).basic_damage;
            attack_distance = ResourceManager.instance.GetWeaponFromWeaponID(equip_weapon_id).basic_distance;
            attack_angle = ResourceManager.instance.GetWeaponFromWeaponID(equip_weapon_id).basic_angle;

            item_SwordSheath.SetActive(false);
            item_Sword.SetActive(true);
        }
        else
        {
            attack_damage = ResourceManager.instance.GetWeaponFromWeaponID(basic_weapon_id).basic_damage;
            attack_distance = ResourceManager.instance.GetWeaponFromWeaponID(basic_weapon_id).basic_distance;
            attack_angle = ResourceManager.instance.GetWeaponFromWeaponID(basic_weapon_id).basic_angle;

            item_SwordSheath.SetActive(true);
            item_Sword.SetActive(false);
        }
        
    }
}
