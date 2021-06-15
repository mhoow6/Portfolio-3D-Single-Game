using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public Dictionary<ushort, bool> quests = new Dictionary<ushort, bool>(); // quest, isCleared?

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Player's Quest Info Load
        foreach(PlayerQuestInfo quest in PlayerQuestTableManager.playerQuestInfoList)
            quests.Add(quest.id, quest.isClear);
    }
}
