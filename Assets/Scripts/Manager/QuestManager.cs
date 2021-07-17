using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Dictionary<QuestInfo, PlayerQuestStateInfo> playerQuests = new Dictionary<QuestInfo, PlayerQuestStateInfo>();
    public QuestInfo currentQuestInfo;
    public PlayerQuestStateInfo currentQuestState;

    [SerializeField]
    private List<ushort> playerQuestsID = new List<ushort>();
    [SerializeField]
    private ushort currentQuestID;
    [SerializeField]
    private bool currentQuestIsClear;
    [SerializeField]
    private bool currentQuestIsAccept;

    private const float QUEST_CHECK_TIME = 3.0F;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Quest Data Check in Unity Editor
        StartCoroutine(CurrentQuestCheck());

        // Quest Update From Table
        QuestListUpdateFromTable();
    }

    public void AddCurrentQuest(QuestInfo questInfo)
    {
        // Ž���� NPC ����Ʈ�� ���� ������ ����Ʈ�� ���
        currentQuestInfo = questInfo;

        currentQuestState = new PlayerQuestStateInfo();

        // Ŭ���� ���´� �翬�� �ƴ�
        currentQuestState.isClear = false;

        // ��ǥ ���� ������ �ʱ�ȭ
        currentQuestState.target_monster_hunted = 0;

        // ����Ʈ ���̵� �ʱ�ȭ
        currentQuestState.quest_id = questInfo.id;

        // ���� ���� ���� �ƴ�
        currentQuestState.isPlayerAccept = false;
    }

    public void AddPlayerQuest(QuestInfo questInfo, PlayerQuestStateInfo questState)
    {
        // ���������� ��ųʸ��� ����
        playerQuests.Add(questInfo, questState);

        // ����Ʈ UI���� �߰�
        HUDManager.instance.quest.AddPlayerQuestInQuestWindow(questInfo.id, questState.isClear);

        // ����Ƽ �ν����Ϳ��� Ȯ���� ����Ʈ ����Ʈ���� �߰�
        playerQuestsID.Add(questInfo.id);

        // NPC ���׼� ����Ʈ�� �������ش�.
        GameManager.instance.npcs.Find(npc =>
        {
            for (int i = 0; i < npc.quests.Count; i++)
            {
                if (npc.quests[i] == questInfo.id)
                    npc.quests.Remove(questInfo.id);
            }

            return false;
        });
    }

    public void EmptyCurrentQuest()
    {
        currentQuestInfo.id = 0;
        currentQuestInfo.start_npc_id = 0;
        currentQuestInfo.end_npc_id = 0;
        currentQuestInfo.start_dialog_id = 0;
        currentQuestInfo.end_dialog_id = 0;
        currentQuestInfo.wait_dialog_id = 0;
        currentQuestInfo.award_start_dialog_id = 0;
        currentQuestInfo.award_end_dialog_id = 0;
        currentQuestInfo.quest_name = "";
        currentQuestInfo.target_monster_id = 0;
        currentQuestInfo.target_monster_count = 0;
        currentQuestInfo.award_item_id = 0;
        currentQuestInfo.award_item_count = 0;
        currentQuestInfo.required_level = 0;
        currentQuestState = null;
    }

    public void DeletePlayerQuest(QuestInfo questInfo)
    {
        // ����Ʈ ��ųʸ����� ����
        playerQuests.Remove(questInfo);

        // ����Ʈ UI������ ����
        HUDManager.instance.quest.DeletePlayerQuestInQuestWindow(questInfo.id);

        // ���ϰ��� ������ ȥ�� ������ ���� ���Ͽ� ���� �÷��̾� ���¸� �����Ѵ�.
        HUDManager.instance.system.SaveGame();

        // ���� ���� ������ ȥ�� ������ ���� PlayerInfoTableManager������ ����
        PlayerInfoTableManager.playerQuests.Remove(PlayerInfoTableManager.playerQuests.Find(quest => quest.quest_id == questInfo.id));

        // ����Ƽ �ν����Ϳ��� Ȯ���� ����Ʈ ����Ʈ���� ����
        playerQuestsID.Remove(questInfo.id);

        // ���� ����Ʈ ����
        EmptyCurrentQuest();
    }

    public void GiveQuestAward(QuestInfo questInfo)
    {
        ushort awardItemID = questInfo.award_item_id;
        byte awardItemType = questInfo.awarad_item_type;
        int awardItemCount = questInfo.award_item_count;

        HUDManager.instance.inventory.itemContent.AddItem(awardItemID, awardItemType, awardItemCount);
        HUDManager.instance.awardCheck.ShowItem(questInfo.quest_name, awardItemID, awardItemType, awardItemCount);
    }

    private IEnumerator CurrentQuestCheck()
    {
        WaitForSeconds wt = new WaitForSeconds(QUEST_CHECK_TIME);

        while (true)
        {
            yield return wt;

            // �ν����� Ȯ�ο�
            currentQuestID = currentQuestInfo.id;

            if (currentQuestInfo.id != 0)
            {
                currentQuestIsClear = currentQuestState.isClear;
                currentQuestIsAccept = currentQuestState.isPlayerAccept;
            }
            else
            {
                currentQuestIsClear = false;
                currentQuestIsAccept = false;
            }
            // �ν����� Ȯ�ο�

            if (playerQuests.Count != 0)
            {
                foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in playerQuests)
                {
                    // [�ν����� Ȯ�ο�] QuestManager�� quest�� playerQuestID�� �����ϱ� ���� ��.
                    if (!playerQuestsID.Contains(quests.Key.id))
                        playerQuestsID.Add(quests.Key.id);

                    // ������� ���� ������ �������� ��ǥ �������� ������ ����Ʈ�� Ŭ���� �� ���̹Ƿ� isClear -> True
                    if (quests.Key.target_monster_count == quests.Value.target_monster_hunted)
                        playerQuests[quests.Key].isClear = true;

                    Debug.Log($"{quests.Key.id}: {quests.Value.isPlayerAccept}, {quests.Value.isClear}, {quests.Value.target_monster_hunted}");
                }
                    
            }
            
        }
    }

    private void QuestListUpdateFromTable()
    {
        if (PlayerInfoTableManager.playerQuests.Count != 0)
        {
            foreach (PlayerQuestStateInfo state in PlayerInfoTableManager.playerQuests)
                if (!playerQuests.TryGetValue(QuestInfoTableManager.GetQuestInfoFromQuestID(state.quest_id), out _))
                    playerQuests.Add(QuestInfoTableManager.GetQuestInfoFromQuestID(state.quest_id), state);
        }

    }
}
