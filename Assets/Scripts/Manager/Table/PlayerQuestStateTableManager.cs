using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ü�� �б� ������ �����͸� ��Ƶ���. ������ �Ͼ ��� Ŭ������ �ϴ°� ����.
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

        PlayerQuestStateInfo playerQuestInfo = new PlayerQuestStateInfo();

        for (int i = 1; i < lines.Count; i++)
        {
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

        PlayerQuestStateInfo playerQuestInfo = new PlayerQuestStateInfo();

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerQuestInfo.quest_id = ushort.Parse(datas[0]);
            playerQuestInfo.isClear = bool.Parse(datas[1]);
            playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);
            playerQuestInfo.target_monster_hunted = int.Parse(datas[3]);

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

        throw new System.NotSupportedException(questID + "�� �ش��ϴ� ����Ʈ�� �����ϴ�.");
    }
}
