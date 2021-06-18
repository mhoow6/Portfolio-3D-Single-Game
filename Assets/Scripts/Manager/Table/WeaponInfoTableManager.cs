using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct WeaponInfo
{
    public ushort id;
    public string prefab_name;
    public string weapon_name;
    public float basic_damage;
    public float basic_distance;
    public byte max_reinforce;
}

public static class WeaponInfoTableManager
{
    public static List<WeaponInfo> weaponInfoList = new List<WeaponInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            WeaponInfo weaponInfo;

            weaponInfo.id = ushort.Parse(datas[0]);
            weaponInfo.prefab_name = datas[1];
            weaponInfo.weapon_name = datas[2];
            weaponInfo.basic_damage = float.Parse(datas[3]);
            weaponInfo.basic_distance = float.Parse(datas[4]);
            weaponInfo.max_reinforce = byte.Parse(datas[5]);

            weaponInfoList.Add(weaponInfo);
        }
    }

    public static WeaponInfo GetWeaponInfoFromWeaponID(ushort weaponID)
    {
        foreach (WeaponInfo weapon in weaponInfoList)
        {
            if (weapon.id == weaponID)
                return weapon;
        }
        throw new System.NotSupportedException("무기중에 " + weaponID.ToString() + " 에 해당하는것이 없습니다.");
    }

    public static string GetPrefabNameFromWeaponID(ushort weaponID)
    {
        foreach (WeaponInfo weapon in weaponInfoList)
        {
            if (weapon.id == weaponID)
                return weapon.prefab_name;
        }
        throw new System.NotSupportedException("무기중에 " + weaponID.ToString() + " 에 해당하는것이 없습니다.");
    }
}
