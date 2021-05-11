using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 캐릭터 테이블에서 HP 로드 -> 각 캐릭터에게 제공
// 무기 테이블에서 무기 공격력, 범위각, 공격거리 로드 -> 각 무기에게 제공

// 주된 역할: 게임 실행시 씬, 몬스터, 캐릭터 배치

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public Weapon weapons;
    public List<Monster> forestMonsters;

    private string forestMonsterPath;
    private string weaponPath;

    private void CreateMonster(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Monster");

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                string objName = datas[0];
                float xPos = float.Parse(datas[1]);
                float zPos = float.Parse(datas[2]);

                GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + objName);
                GameObject obj = Instantiate(_obj);

                Monster monster = Monster.AddMonsterComponent(obj, objName);
                monster.name = objName;

                monster.transform.position = Utility.RayToDown(new Vector3(xPos, 0, zPos));
                forestMonsters.Add(monster);

                monster.transform.SetParent(parent.transform);
            }
        }
    }

    private void ResourceFromWeaponTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                switch (datas[0])
                {
                    case "punch_attack":
                        weapons.punch_attack_damage = int.Parse(datas[1]);
                        weapons.punch_attack_distance = float.Parse(datas[2]);
                        weapons.punch_attack_angle = float.Parse(datas[3]);
                        break;

                    case "combat_attack":
                        weapons.combat_attack_damage = int.Parse(datas[1]);
                        weapons.combat_attack_distance = float.Parse(datas[2]);
                        weapons.combat_attack_angle = float.Parse(datas[3]);
                        break;

                    case "spider_attack":
                        weapons.spider_attack_damage = int.Parse(datas[1]);
                        weapons.spider_attack_distance = float.Parse(datas[2]);
                        weapons.spider_attack_angle = float.Parse(datas[3]);
                        break;
                }
            }
        }
    }

    private void Awake()
    {
        instance = this;
        weapons = new Weapon();
        forestMonsterPath = Application.dataPath + "/Resources/Tables/ForestMonster.csv";
        weaponPath = Application.dataPath + "/Resources/Tables/Weapon.csv";

        ResourceFromWeaponTable(weaponPath);
        CreateMonster(forestMonsterPath);
    }

    private void CheckWeaponData()
    {
        Debug.Log(weapons.punch_attack_damage);
        Debug.Log(weapons.punch_attack_distance);
        Debug.Log(weapons.punch_attack_angle);

        Debug.Log(weapons.combat_attack_damage);
        Debug.Log(weapons.combat_attack_distance);
        Debug.Log(weapons.combat_attack_angle);

        Debug.Log(weapons.spider_attack_damage);
        Debug.Log(weapons.spider_attack_distance);
        Debug.Log(weapons.spider_attack_angle);
    }
}
