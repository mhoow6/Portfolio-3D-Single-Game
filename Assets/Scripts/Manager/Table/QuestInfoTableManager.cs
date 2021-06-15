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
    public string quest_name;
    public ushort target_monster_id;
    public int target_monster_count;
    public ushort award_item_id;
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
            questInfo.quest_name = datas[5];
            questInfo.target_monster_id = ushort.Parse(datas[6]);
            questInfo.target_monster_count = int.Parse(datas[7]);
            questInfo.award_item_id = ushort.Parse(datas[8]);
            questInfo.award_item_count = int.Parse(datas[9]);
            questInfo.required_level = byte.Parse(datas[10]);

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
}
