using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuestInfo
{
    public ushort id;
    public ushort start_npc_id;
    public ushort end_npc_id;
    public ushort start_dialog_id;
    public ushort end_dialog_id;
    public ushort wait_dialog_id;
    public ushort award_start_dialog_id;
    public ushort award_end_dialog_id;
    public string quest_name;
    public ushort target_monster_id;
    public int target_monster_count;
    public ushort award_item_id;
    public byte awarad_item_type;
    public int award_item_count;
    public byte required_level;
}


public static class QuestInfoTableManager
{
    public static List<QuestInfo> questInfoList = new List<QuestInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            QuestInfo questInfo;

            questInfo.id = ushort.Parse(datas[0]);
            questInfo.start_npc_id = ushort.Parse(datas[1]);
            questInfo.end_npc_id = ushort.Parse(datas[2]);
            questInfo.start_dialog_id = ushort.Parse(datas[3]);
            questInfo.end_dialog_id = ushort.Parse(datas[4]);
            questInfo.wait_dialog_id = ushort.Parse(datas[5]);
            questInfo.award_start_dialog_id = ushort.Parse(datas[6]);
            questInfo.award_end_dialog_id = ushort.Parse(datas[7]);
            questInfo.quest_name = datas[8];
            questInfo.target_monster_id = ushort.Parse(datas[9]);
            questInfo.target_monster_count = int.Parse(datas[10]);
            questInfo.award_item_id = ushort.Parse(datas[11]);
            questInfo.awarad_item_type = byte.Parse(datas[12]);
            questInfo.award_item_count = int.Parse(datas[13]);
            questInfo.required_level = byte.Parse(datas[14]);

            questInfoList.Add(questInfo);
        }
    }

    public static byte GetRequiredLevelFromQuestID(ushort questID)
    {
        foreach (QuestInfo questInfo in questInfoList)
        {
            if (questInfo.id == questID)
                return questInfo.required_level;
        }

        throw new System.NotSupportedException(questID + " 에 해당하는 퀘스트가 없습니다.");
    }

    public static string GetQuestNameFromQuestID(ushort questID)
    {
        foreach (QuestInfo questInfo in questInfoList)
        {
            if (questInfo.id == questID)
                return questInfo.quest_name;
        }

        throw new System.NotSupportedException(questID + " 에 해당하는 퀘스트가 없습니다.");
    }

    public static ushort GetQuestIDFromQuestName(string questName)
    {
        foreach (QuestInfo questInfo in questInfoList)
        {
            if (questInfo.quest_name == questName)
                return questInfo.id;
        }

        throw new System.NotSupportedException(questName + " 에 해당하는 퀘스트가 없습니다.");
    }

    public static QuestInfo GetQuestInfoFromQuestID(ushort questID)
    {
        foreach (QuestInfo questInfo in questInfoList)
        {
            if (questInfo.id == questID)
                return questInfo;
        }

        throw new System.NotSupportedException(questID + " 에 해당하는 퀘스트가 없습니다.");
    }
}
