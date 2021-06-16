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
    public List<QuestInfo> quests = new List<QuestInfo>();
    public List<DialogInfo> dialogs = new List<DialogInfo>();
    public NavMeshAgent agent;
    public Image questIconImage;
    public GameObject questIcon;

    private const int AGENT_PRIORITY = 49;
    private const float QUEST_CHECK_DURATION = 3.0f;
    private const float QUEST_VISIBLE_DISTANCE = 15.0f;

    public bool _isQuestExists { get => isQuestExists; }

    [SerializeField]
    private bool isQuestExists;

    public Vector3 headLocalPos
    {
        get => new Vector3(0, 2.0f, 0);
    }
    
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
            if (questInfo.start_npc_id == id)
                quests.Add(questInfo);
        }

        // NPC'S Dialog Load
        foreach (DialogInfo dialogInfo in DialogInfoTableManager.dialogInfoList)
        {
            if (dialogInfo.npc_id == id)
                dialogs.Add(dialogInfo);
        }

        // Get Bound
        bound = GetBoundFromSkinnedMeshRenderer(this).Value;

        // Checking NPC Quest
        StartCoroutine(QuestUpdate(QUEST_CHECK_DURATION));
    }

    private IEnumerator QuestUpdate(float checkDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(checkDuration);

        yield return null;

        while (true)
        {
            // If Quest is exists at least one
            if (quests.Count != 0)
            {
                for (int i = 0; i < quests.Count; i++)
                {
                    if (!PlayerQuestStateTableManager.playerQuestStateList.Find(quest => quest.id == quests[i].id).isClear &&
                        QuestInfoTableManager.GetRequiredLevelFromQuestID(quests[i].id) <= GameManager.instance.controller.player.level)
                        isQuestExists = true;
                    else
                        isQuestExists = false;
                }
            }

            yield return wt;
        }
    }
}

