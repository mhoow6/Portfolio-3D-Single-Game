using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ConsumeItemInfo
{
    public ushort id;
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
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            ConsumeItemInfo consumeInfo;

            consumeInfo.id = ushort.Parse(datas[0]);
            consumeInfo.item_name = datas[1];
            consumeInfo.hp_heal = float.Parse(datas[2]);
            consumeInfo.mp_heal = float.Parse(datas[3]);
            consumeInfo.damage_boost = float.Parse(datas[4]);
            consumeInfo.max_count = byte.Parse(datas[5]);

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
}
