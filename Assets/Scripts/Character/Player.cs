using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    void Awake()
    {
        HP = 100; // юс╫ц
    }

    public void Attack(int ani_id)
    {

        switch (ani_id)
        {
            case (int)PlayerAnimation.AniType.ATTACK:
                attack_damage = ResourceManager.instance.weapons.punch_attack_damage;
                attack_distance = ResourceManager.instance.weapons.punch_attack_distance;
                attack_angle = ResourceManager.instance.weapons.punch_attack_angle;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
                attack_damage = ResourceManager.instance.weapons.combat_attack_damage;
                attack_distance = ResourceManager.instance.weapons.combat_attack_distance;
                attack_angle = ResourceManager.instance.weapons.combat_attack_angle;
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
                mob.HP -= attack_damage;
            }
        }
    }

    public void WeaponSwitch()
    {

    }
}
