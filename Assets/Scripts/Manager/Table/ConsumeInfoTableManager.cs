using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public struct ConsumeItemInfo
{
    public ushort id;
    public string prefab_name;
    public string item_name;
    public float hp_heal;
    public float mp_heal;
    public float damage_boost;
    public byte max_count;
}

public static class ConsumeInfoTableManager
{
    public static List<ConsumeItemInfo> consumeInfoList = new List<ConsumeItemInfo>();

    public static void LoadTable(string filePath)
    {
        string lines = TableManager.instance.GetLinesWithFileStream(filePath);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(lines);

        XmlNodeList items = xmlDoc.SelectNodes("dataroot/ConsumeItem");

        foreach (XmlNode node in items)
        {
            ConsumeItemInfo consumeInfo;

            consumeInfo.id = ushort.Parse(node.SelectSingleNode("id").InnerText);
            consumeInfo.prefab_name = node.SelectSingleNode("prefab_name").InnerText;
            consumeInfo.item_name = node.SelectSingleNode("item_name").InnerText;
            consumeInfo.hp_heal = float.Parse(node.SelectSingleNode("hp_heal").InnerText);
            consumeInfo.mp_heal = float.Parse(node.SelectSingleNode("mp_heal").InnerText);
            consumeInfo.damage_boost = float.Parse(node.SelectSingleNode("damage_boost").InnerText);
            consumeInfo.max_count = byte.Parse(node.SelectSingleNode("max_count").InnerText);

            consumeInfoList.Add(consumeInfo);
        }
    }

    public static ConsumeItemInfo GetConsumeItemInfoFromID(ushort itemID)
    {
        foreach (ConsumeItemInfo consumeInfo in consumeInfoList)
        {
            if (itemID == consumeInfo.id)
                return consumeInfo;
        }

        throw new System.NotSupportedException(itemID + " 에 해당하는 소비 아이템은 없습니다.");
    }

    public static string GetPrefabNameInfoFromID(ushort itemID)
    {
        foreach (ConsumeItemInfo consumeInfo in consumeInfoList)
        {
            if (itemID == consumeInfo.id)
                return consumeInfo.prefab_name;
        }

        throw new System.NotSupportedException(itemID + " 에 해당하는 소비 아이템은 없습니다.");
    }
}
