using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct WeaponInfo
{
    public ushort id;
    public string weapon_name;
    public float basic_damage;
    public float basic_distance;
    public byte max_reinforce;
}

public static class WeaponInfoTableManager
{
    public static List<WeaponInfo> weaponInfoList = new List<WeaponInfo>();

    public static void LoadTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            sr.ReadLine(); // 첫 레코드 제외

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                WeaponInfo weaponInfo;

                weaponInfo.id = ushort.Parse(datas[0]);
                weaponInfo.weapon_name = datas[1];
                weaponInfo.basic_damage = float.Parse(datas[2]);
                weaponInfo.basic_distance = float.Parse(datas[3]);
                weaponInfo.max_reinforce = byte.Parse(datas[4]);

                weaponInfoList.Add(weaponInfo);
            }
            sr.Close();
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
}
