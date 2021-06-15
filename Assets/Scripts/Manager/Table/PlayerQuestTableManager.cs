using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerQuestInfo
{
    public ushort id;
    public bool isClear;
}

public static class PlayerQuestTableManager
{
    public static List<PlayerQuestInfo> playerQuestInfoList = new List<PlayerQuestInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            PlayerQuestInfo playerQuestInfo;

            playerQuestInfo.id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);

            playerQuestInfoList.Add(playerQuestInfo);
        }
    }

    public static void LoadTempTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTempTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            PlayerQuestInfo playerQuestInfo;

            playerQuestInfo.id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);

            playerQuestInfoList.Add(playerQuestInfo);
        }
    }
}
