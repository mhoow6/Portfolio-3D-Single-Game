using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    private void Awake()
    {
        HP = 150; // �ӽ�
    }

    // ���͸��� �״� ����� �ٸ� �� �����Ƿ� virtual
    virtual public void Dead()
    {
        if (HP < 0)
            HP = 0;

        Invoke("Disabled", 5f);
    }

    protected void Disabled()
    {
        gameObject.SetActive(false);
    }

    // Factory Method
    public static Monster AddMonsterComponent(GameObject obj, string objName)
    {
        Monster monster = null;

        switch (objName)
        {
            case "Polygonal_Metalon_Purple":
                monster = obj.AddComponent<Spider>();
                return monster;

            case "Polygonal_Metalon_Green":
                monster = obj.AddComponent<Spider>();
                return monster;

            case "Polygonal_Metalon_Red":
                monster = obj.AddComponent<Spider>();
                return monster;
        }

        throw new System.NotSupportedException("�����߿�" + objName + " �� �����ϴ�.");
    }
}

public class CustomDummy : Monster
{
}

public class NormalMob : Monster
{
}

public class Spider : NormalMob
{
    private void Start()
    {
        attack_damage = ResourceManager.instance.weapons.spider_attack_damage;
        attack_distance = ResourceManager.instance.weapons.spider_attack_distance;
        attack_angle = ResourceManager.instance.weapons.spider_attack_angle;
    }
}
