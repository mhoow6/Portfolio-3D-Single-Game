using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 주된 역할: 게임 실행시 씬, 몬스터, 캐릭터 배치
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    // WeaponInfoTableManager로 옮길 것
    public List<Weapon> weapons;

    private string forestMonsterPath;
    private string weaponPath;
    private string playerPath;
    private string npcPath;

    private void Awake()
    {
        instance = this;
        weapons = new List<Weapon>();
        forestMonsterPath = Application.dataPath + "/Resources/Tables/ForestMonsterPosition.csv";
        weaponPath = Application.dataPath + "/Resources/Tables/WeaponInfo.csv";

        ResourceFromWeaponTable(weaponPath);
        // CreateMonster(forestMonsterPath);
    }

    private void CreateMonster(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Monster");

            sr.ReadLine(); // 첫 레코드 스킵

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                ushort index = ushort.Parse(datas[0]);
                ushort id = ushort.Parse(datas[1]);
                float xPos = float.Parse(datas[2]);
                float yPos = float.Parse(datas[3]);
                float zPos = float.Parse(datas[4]);
                float xRot = float.Parse(datas[5]);
                float yRot = float.Parse(datas[6]);
                float zRot = float.Parse(datas[7]);
                float xScale = float.Parse(datas[8]);
                float yScale = float.Parse(datas[9]);
                float zScale = float.Parse(datas[10]);

                string mobName = GetMonsterNametoID(id);

                GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + mobName);
                GameObject obj = Instantiate(_obj);
                Monster monster = Monster.AddMonsterComponent(obj, id);

                monster.index = index;
                monster.id = id;
                monster.name = mobName;

                monster.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos)); // 떠 있는 현상 방지
                monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                monster.transform.localScale = new Vector3(xScale, yScale, zScale);

                monster.transform.SetParent(parent.transform);
            }
        }
    }

    // WeaponInfoTableManager로 옮길 것
    private void ResourceFromWeaponTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                if (datas[0] == "id")
                    continue;

                Weapon weapon = new Weapon();
                weapon.id = ushort.Parse(datas[0]);
                weapon.weapon_name = datas[1];
                weapon.basic_damage = float.Parse(datas[2]);
                weapon.basic_distance = float.Parse(datas[3]);
                weapon.basic_angle = float.Parse(datas[4]);

                weapons.Add(weapon);
            }
        }
    }

    private void CheckWeaponData()
    {
        Debug.Log(weapons[0].id);
    }

    public Weapon GetWeaponFromWeaponID(ushort weaponID)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.id == weaponID)
                return weapon;
        }
        throw new System.NotSupportedException("무기중에" + weaponID + " 은 없습니다.");
    }

    private static string GetMonsterNametoID(ushort mobID)
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.monster_name;
        }

        throw new System.NotSupportedException(mobID + "에 해당하는 몬스터는 없습니다.");
    }
}
