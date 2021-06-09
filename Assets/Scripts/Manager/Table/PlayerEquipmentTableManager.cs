using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentIndex
{
    WEAPON = 0,
    QUICKITEM
}

public static class PlayerEquipmentTableManager
{
    public static ItemInfo[] playerEquipment = new ItemInfo[2];

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerEquipment[i - 1].index = short.Parse(datas[0]);
            playerEquipment[i - 1].item_type = byte.Parse(datas[1]);
            playerEquipment[i - 1].id = ushort.Parse(datas[2]);
            playerEquipment[i - 1].icon_name = datas[3];
            playerEquipment[i - 1].count = ushort.Parse(datas[4]);
            playerEquipment[i - 1].reinforce_level = byte.Parse(datas[5]);
        }
    }

    public static ref ItemInfo[] GetPlayerEquipmentFromTable()
    {
        return ref playerEquipment;
    }
}
