using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mobType
{
    Polygonal_Metalon_Purple = 1000,
    Polygonal_Metalon_Green,
    Polygonal_Metalon_Red,
    Character_BR_BigOrk_01,
    Character_BR_BigOrk_02,
    Character_BR_PigButcher_01,
    Character_BR_FortGolem_01
}

public class Monster : Character
{
    public ushort id;
    public ushort index;
    public byte monster_type;
    public float run_speed;

    private void Awake()
    {
        hp = 100; // TestZone Only
    }

    virtual public void Dead()
    {
        if (hp < 0)
            hp = 0;

        Invoke("Disabled", 5f);
    }

    protected void Disabled()
    {
        gameObject.SetActive(false);
    }

    public static Monster AddMonsterComponent(GameObject obj, ushort mobID)
    {
        Monster monster = null;

        switch (mobID)
        {
            case (ushort)mobType.Polygonal_Metalon_Purple:
                monster = obj.AddComponent<Spider>();
                return monster;

            case (ushort)mobType.Polygonal_Metalon_Green:
                monster = obj.AddComponent<Spider>();
                return monster;

            case (ushort)mobType.Polygonal_Metalon_Red:
                monster = obj.AddComponent<Spider>();
                return monster;

            case (ushort)mobType.Character_BR_PigButcher_01:
                monster = obj.AddComponent<Pig>();
                return monster;

            case (ushort)mobType.Character_BR_BigOrk_01:
                monster = obj.AddComponent<Ork>();
                return monster;

            case (ushort)mobType.Character_BR_BigOrk_02:
                monster = obj.AddComponent<Ork>();
                return monster;

            case (ushort)mobType.Character_BR_FortGolem_01:
                monster = obj.AddComponent<Golem>();
                return monster;
        }

        throw new System.NotSupportedException(mobID.ToString() + " �� �ش��ϴ� ���Ͱ� ���� ��ũ��Ʈ �߰��� �����߽��ϴ�.");
    }
}

public class CommonMonster : Monster
{
}

public class EliteMonster : Monster
{
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
}

public class BossMonster : Monster
{
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
    public float skill_2_damage;
    public float skill_2_distance;
    public float skill_2_angle;
    public float skill_3_damage;
    public float skill_3_distance;
    public float skill_3_angle;
}

public class Spider : CommonMonster
{
    private void Start()
    {
        // �ݺ�
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == (ushort)mobType.Polygonal_Metalon_Purple)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                return;
            }
        }
        // �ݺ�
    }
}

public class Pig : CommonMonster
{
    private void Start()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == (ushort)mobType.Character_BR_PigButcher_01)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                return;
            }
        }
    }
}

public class Ork : EliteMonster
{
    private void Start()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == (ushort)mobType.Character_BR_BigOrk_01)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                return;
            }
        }
    }
}

public class Golem: BossMonster
{
    private void Start()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == (ushort)mobType.Character_BR_FortGolem_01)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;
                skill_2_damage = mobinfo.skill_2_damage;
                skill_2_distance = mobinfo.skill_2_distance;
                skill_2_angle = mobinfo.skill_2_angle;
                skill_3_damage = mobinfo.skill_3_damage;
                skill_3_distance = mobinfo.skill_3_distance;
                skill_3_angle = mobinfo.skill_3_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                return;
            }
        }
    }
}
