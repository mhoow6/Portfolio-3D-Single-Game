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
        // 탐색한 NPC 퀘스트를 현재 진행중 퀘스트로 등록
        currentQuestInfo = questInfo;

        currentQuestState = new PlayerQuestStateInfo();

        // 클리어 상태는 당연히 아님
        currentQuestState.isClear = false;

        // 목표 몬스터 마리수 초기화
        currentQuestState.target_monster_hunted = 0;

        // 퀘스트 아이디 초기화
        currentQuestState.quest_id = questInfo.id;

        // 아직 받은 상태 아님
        currentQuestState.isPlayerAccept = false;
    }

    public void AddPlayerQuest(QuestInfo questInfo, PlayerQuestStateInfo questState)
    {
        // 최종적으로 딕셔너리에 삽입
        playerQuests.Add(questInfo, questState);

        // 퀘스트 UI에도 추가
        HUDManager.instance.quest.AddPlayerQuestInQuestWindow(questInfo.id, questState.isClear);

        // 유니티 인스펙터에서 확인할 퀘스트 리스트에도 추가
        playerQuestsID.Add(questInfo.id);

        // NPC 한테서 퀘스트를 제거해준다.
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
        // 퀘스트 딕셔너리에서 삭제
        playerQuests.Remove(questInfo);

        // 퀘스트 UI에서도 삭제
        HUDManager.instance.quest.DeletePlayerQuestInQuestWindow(questInfo.id);

        // 유니티 인스펙터에서 확인할 퀘스트 리스트에도 추가
        playerQuestsID.Remove(questInfo.id);

        // 현재 퀘스트 종료
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

            // 인스펙터 확인용
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
            // 인스펙터 확인용

            if (playerQuests.Count != 0)
            {
                foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in playerQuests)
                {
                    // [인스펙터 확인용] QuestManager의 quest를 playerQuestID에 갱신하기 위한 것.
                    if (!playerQuestsID.Contains(quests.Key.id))
                        playerQuestsID.Add(quests.Key.id);

                    // 현재까지 잡은 몬스터의 마릿수가 목표 마릿수랑 같으면 퀘스트가 클리어 된 것이므로 isClear -> True
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
                playerQuests.Add(QuestInfoTableManager.GetQuestInfoFromQuestID(state.quest_id), state);
        }

    }
}
