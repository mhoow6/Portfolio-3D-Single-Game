using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구조체는 읽기 전용의 데이터만 담아두자. 변경이 일어날 경우 클래스로 하는게 낫다.
public class PlayerQuestStateInfo
{
    public ushort quest_id;
    public bool isClear;
    public bool isPlayerAccept;
    public int target_monster_hunted;
}

public static class PlayerQuestStateTableManager
{
    public static List<PlayerQuestStateInfo> playerQuestStateList = new List<PlayerQuestStateInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            PlayerQuestStateInfo playerQuestInfo = new PlayerQuestStateInfo();

            string[] datas = lines[i].Split(',');

            playerQuestInfo.quest_id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);
            playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);
            playerQuestInfo.target_monster_hunted = int.Parse(datas[3]);

            playerQuestStateList.Add(playerQuestInfo);
        }
    }

    public static void LoadTempTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTempTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            PlayerQuestStateInfo playerQuestInfo = new PlayerQuestStateInfo();
            string[] datas = lines[i].Split(',');

            playerQuestInfo.quest_id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);
            playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);
            playerQuestInfo.target_monster_hunted = int.Parse(datas[3]);

            if (playerQuestStateList.Find(quest => quest.quest_id == playerQuestInfo.quest_id) == null)
                playerQuestStateList.Add(playerQuestInfo);
        }
    }

    public static bool isThisQuestClear(ushort questID)
    {
        foreach (PlayerQuestStateInfo state in playerQuestStateList)
        {
            if (state.quest_id == questID)
                return state.isClear;
        }

        throw new System.NotSupportedException(questID + "에 해당하는 퀘스트가 없습니다.");
    }
}
