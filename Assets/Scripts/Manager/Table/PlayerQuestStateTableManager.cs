using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerQuestStateInfo
{
    public ushort id;
    public bool isClear;
    public bool isPlayerAccept;
}

public static class PlayerQuestStateTableManager
{
    public static List<PlayerQuestStateInfo> playerQuestStateList = new List<PlayerQuestStateInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            PlayerQuestStateInfo playerQuestInfo;

            playerQuestInfo.id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);
            playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);

            playerQuestStateList.Add(playerQuestInfo);
        }
    }

    public static void LoadTempTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTempTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            PlayerQuestStateInfo playerQuestInfo;

            playerQuestInfo.id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);
            playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);

            playerQuestStateList.Add(playerQuestInfo);
        }
    }

    public static bool isThisQuestClear(ushort questID)
    {
        foreach (PlayerQuestStateInfo state in playerQuestStateList)
        {
            if (state.id == questID)
                return state.isClear;
        }

        throw new System.NotSupportedException(questID + "에 해당하는 퀘스트가 없습니다.");
    }
}
