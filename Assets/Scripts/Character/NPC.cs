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
    public NavMeshAgent agent;
    public Image questIconImage;
    public GameObject questIcon;
    public bool isPlayerQuestAccept;

    private const int AGENT_PRIORITY = 49;
    private const float QUEST_CHECK_DURATION = 3.0f;

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

        // Get Bound
        bound = GetBoundFromSkinnedMeshRenderer(this).Value;

        // Checking NPC Quest
        StartCoroutine(QuestUpdate(QUEST_CHECK_DURATION));

        // questIconImage From PoolManager
        questIconImage = PoolManager.instance.CreateQuestIconImage();
    }

    private void Update()
    {
        QuestIconUpdate();
        Detector();
    }

    private void QuestIconUpdate()
    {
        if (isQuestExists)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(questIcon.transform.position);
            questIconImage.transform.position = screenPos;

            if (questIconImage.transform.position.x > -30f && questIconImage.transform.position.x < 1950f &&
                questIconImage.transform.position.y > 400f && questIconImage.transform.position.y < 800f && currentDistanceWithPlayer <= 10f)
                questIconImage.gameObject.SetActive(true);
            else
                questIconImage.gameObject.SetActive(false);
        }

        if (!isQuestExists && !isPlayerQuestAccept)
            questIconImage.gameObject.SetActive(false);
    }

    private IEnumerator QuestUpdate(float checkDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(checkDuration);

        yield return null;

        while (true)
        {
            for (int i = 0; i < quests.Count; i++)
            {
                if (QuestManager.instance.quests.TryGetValue(quests[i].id, out bool clear) &&
                    QuestInfoTableManager.GetRequiredLevelFromQuestID(quests[i].id) <= GameManager.instance.controller.player.level)
                    isQuestExists = !clear;
                else
                    isQuestExists = clear;
            }
            yield return wt;
        }
    }
}

