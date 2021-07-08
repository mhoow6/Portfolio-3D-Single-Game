using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

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
        string lines = TableManager.instance.GetLinesWithFileStream(filePath);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(lines);

        XmlNodeList weapons = xmlDocument.SelectNodes("dataroot/Weapon");

        foreach (XmlNode node in weapons)
        {
            WeaponInfo weaponInfo;

            weaponInfo.id = ushort.Parse(node.SelectSingleNode("id").InnerText);
            weaponInfo.prefab_name = node.SelectSingleNode("prefab_name").InnerText;
            weaponInfo.weapon_name = node.SelectSingleNode("weapon_name").InnerText;
            weaponInfo.basic_damage = float.Parse(node.SelectSingleNode("basic_damage").InnerText);
            weaponInfo.basic_distance = float.Parse(node.SelectSingleNode("basic_distance").InnerText);
            weaponInfo.max_reinforce = byte.Parse(node.SelectSingleNode("max_reinforce").InnerText);

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
