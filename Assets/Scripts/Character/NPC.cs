using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : Character
{    
    public ushort id;
    public ushort index;
    public byte npc_type;
    public string npc_name;
    public List<ushort> quests = new List<ushort>();
    public List<ushort> dialogs = new List<ushort>();
    public NavMeshAgent agent;

    private const int AGENT_PRIORITY = 49;
    private const float QUEST_CHECK_DURATION = 3.0f;

    public bool _isQuestExists { get => isQuestExists; }
    [SerializeField]
    private bool isQuestExists;
    
    private void Start()
    {
        // NPC Info Load
        foreach (NpcInfo npcinfo in NPCInfoTableManager.npcInfoList)
        {
            if (npcinfo.id == id)
            {
                npc_name = npcinfo.npc_name;
                npc_type = npcinfo.npc_type;
                agent.avoidancePriority = AGENT_PRIORITY;
            }
        }

        // NPC's Quest Load
        foreach (QuestInfo questInfo in QuestInfoTableManager.questInfoList)
        {
            if (questInfo.start_npc_id == id && !QuestManager.instance.playerQuests.TryGetValue(questInfo, out _))
                quests.Add(questInfo.id);
        }

        // NPC'S Dialog Load
        foreach (DialogInfo dialogInfo in DialogInfoTableManager.dialogInfoList)
        {
            if (dialogInfo.npc_id == id)
                dialogs.Add(dialogInfo.id);
        }

        // Get Bound
        bound = GetBoundFromSkinnedMeshRenderer(this).Value;

        // Checking NPC Quest
        isQuestExists = PlayerQuestStateCheckFromTable();

        // Real Time Quest Check
        StartCoroutine(QuestRealTimeCheck(QUEST_CHECK_DURATION));
    }

    private bool PlayerQuestStateCheckFromTable()
    {
        if (quests.Count != 0)
            return true;
        else
            return false;
    }

    private IEnumerator QuestRealTimeCheck(float checkingTime)
    {
        WaitForSeconds wt = new WaitForSeconds(checkingTime);

        while (true)
        {
            yield return wt;

            if (quests.Count != 0)
            {
                foreach (ushort quest in quests)
                {
                    if (QuestManager.instance.playerQuests.TryGetValue(QuestInfoTableManager.GetQuestInfoFromQuestID(quest), out _))
                        isQuestExists = false;
                    else
                        isQuestExists = true;
                }
            }
            else
                isQuestExists = false;
        }
    }
}

