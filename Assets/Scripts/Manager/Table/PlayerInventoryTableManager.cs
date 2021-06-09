using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    NONE = 0,
    EQUIPMENT,
    CONSUME,
    QUEST,
    REINFORCE
}

public struct ItemInfo
{
    public short index;
    public byte item_type;
    public ushort id;
    public string icon_name;
    public ushort count;
    public byte reinforce_level;
}

public static class PlayerInventoryTableManager
{
    public static ItemInfo[] playerInventory = new ItemInfo[50];
    public const string spritePath = "Sprite/";

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerInventory[i - 1].index = short.Parse(datas[0]);
            playerInventory[i - 1].item_type = byte.Parse(datas[1]);
            playerInventory[i - 1].id = ushort.Parse(datas[2]);
            playerInventory[i - 1].icon_name = datas[3];
            playerInventory[i - 1].count = ushort.Parse(datas[4]);
            playerInventory[i - 1].reinforce_level = byte.Parse(datas[5]);
        }
    }

    public static ref ItemInfo[] GetPlayerInventoryFromTable()
    {
        return ref playerInventory;
    }
}
