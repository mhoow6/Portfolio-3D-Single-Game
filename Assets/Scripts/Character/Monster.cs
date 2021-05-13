using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mobType
{
    Polygonal_Metalon_Purple = 1000,
    Polygonal_Metalon_Green,
    Polygonal_Metalon_Red,
    Character_BR_BigOrk_01,
    Character_BR_BigOrk_02
}

public class Monster : Character
{
    public ushort index;
    public ushort id;

    private void Awake()
    {
        hp = 100; // 임시
    }

    // 몬스터마다 죽는 방식이 다를 수 있으므로 virtual
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

    // Factory Method
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

            case (ushort)mobType.Character_BR_BigOrk_01:
                monster = obj.AddComponent<Ork>();
                return monster;

            case (ushort)mobType.Character_BR_BigOrk_02:
                monster = obj.AddComponent<Ork>();
                return monster;
        }

        throw new System.NotSupportedException(mobID + "에 해당하는 몬스터가 없어 스크립트 추가에 실패했습니다.");
    }
}

public class CustomDummy : Monster
{
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

public class Spider : CommonMonster
{
    private void Start()
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id >= (ushort)mobType.Polygonal_Metalon_Purple && mobinfo.id <= (ushort)mobType.Polygonal_Metalon_Red)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
            }
        }
    }
}

public class Ork : EliteMonster
{
    private void Start()
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id >= (ushort)mobType.Character_BR_BigOrk_01 && mobinfo.id <= (ushort)mobType.Character_BR_BigOrk_02)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;

            }
        }
    }
}
