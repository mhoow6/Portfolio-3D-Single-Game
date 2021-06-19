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

            if (playerQuestStateList.Count != 0)
            {
                for (int j = 0; j < playerQuestStateList.Count; j++)
                {
                    // 테이블에서 playerQuestStateList에 있는 퀘스트 중 같은 것이 발견되면 건너뛰자.
                    // [참고] 데이터 갱신 필요없음. 참조변수라 이미 QuestManager에서 State가 변경될 때 같이 변경됨
                    if (ushort.Parse(datas[0]) == playerQuestStateList[j].quest_id)
                        continue;
                }
            }
            else
            {
                playerQuestInfo.quest_id = ushort.Parse(datas[0]);
                playerQuestInfo.isClear = bool.Parse(datas[1]);
                playerQuestInfo.isPlayerAccept = bool.Parse(datas[2]);
                playerQuestInfo.target_monster_hunted = int.Parse(datas[3]);

                playerQuestStateList.Add(playerQuestInfo);
            }
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
