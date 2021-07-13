using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindowManager : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform questContent;
    public QuestSlot questDummy;
    public List<QuestSlot> questSlots = new List<QuestSlot>();
    public ControlSlot homebtn;
    public bool isQuestWindowOn;

    [HideInInspector]
    public Color completeButtonColor = new Color(255, 255, 255, 1);
    [HideInInspector]
    public Color completeButtonTextColor = new Color(0, 0, 0, 1);
    [HideInInspector]
    public Color defaultButtonColor = new Color(0, 0, 0, 0.5f);
    [HideInInspector]
    public Color defaultButtonTextColor = new Color(255, 255, 255, 0.5f);

    private Sprite[] spr;

    private void OnEnable()
    {
        isQuestWindowOn = true;
        LoadPlayerQuest();
        HUDManager.instance.system.TimeStop();
    }

    private void OnDisable()
    {
        isQuestWindowOn = false;
        HUDManager.instance.system.TimeResume();
    }

    public void LoadPlayerQuest()
    {
        if (PlayerInfoTableManager.playerQuests.Count == 0)
            return;

        for (int i = 0; i < PlayerInfoTableManager.playerQuests.Count; i++)
            AddPlayerQuestInQuestWindow(PlayerInfoTableManager.playerQuests[i].quest_id, PlayerInfoTableManager.playerQuests[i].isClear);
    }

    public void AddPlayerQuestInQuestWindow(ushort questID, bool isClear)
    {
        QuestSlot existQuest = questSlots.Find(quest => quest.questID == questID);
        QuestSlot questSlot = null;
        QuestInfo questInfo;

        if (existQuest != null)
        {
            if (QuestManager.instance.playerQuests.TryGetValue(QuestInfoTableManager.GetQuestInfoFromQuestID(questID), out PlayerQuestStateInfo state))
            {
                existQuest.target_monster_hunted = state.target_monster_hunted;
                existQuest.isClear = state.isClear;

                existQuest.questTitle.text = existQuest.quest_name + "\n" +
                "<size=26><color=#BEB5B6>" +
                MonsterInfoTableManager.GetMonsterNameFromID(existQuest.target_monster_id) + " " +
                state.target_monster_hunted + " / " +
                existQuest.target_monster_count +
                "</color></size>";

                existQuest.gameObject.SetActive(true);

                if (existQuest.isClear)
                    QuestButtonChange(existQuest, state.isClear);
            }

        }
        else
        {
            QuestSlot usedSlot = questSlots.Find(slot => !slot.gameObject.activeSelf);

            if (usedSlot != null)
                questSlot = usedSlot;
            else
                questSlot = Instantiate(questDummy); // 오브젝트 풀링

            questInfo = QuestInfoTableManager.GetQuestInfoFromQuestID(questID);

            if (QuestManager.instance.playerQuests.TryGetValue(QuestInfoTableManager.GetQuestInfoFromQuestID(questID), out PlayerQuestStateInfo state))
            {
                questSlot.name = "Quest";
                questSlot.questID = questID;
                questSlot.quest_name = questInfo.quest_name;
                questSlot.target_monster_id = questInfo.target_monster_id;
                questSlot.target_monster_count = questInfo.target_monster_count;
                questSlot.target_monster_hunted = state.target_monster_hunted;
                questSlot.isClear = state.isClear;

                questSlot.questTitle.text = questInfo.quest_name + "\n" +
                "<size=26><color=#BEB5B6>" +
                MonsterInfoTableManager.GetMonsterNameFromID(questInfo.target_monster_id) + " " +
                questSlot.target_monster_hunted + " / " +
                questInfo.target_monster_count +
                "</color></size>";

                if (questSlot.isClear)
                    QuestButtonChange(questSlot, state.isClear);

                questSlot.gameObject.SetActive(true);
                questSlot.transform.SetParent(questContent);

                questSlots.Add(questSlot);
                return;
            }

            questSlot.name = "Quest";
            questSlot.questID = questID;
            questSlot.quest_name = questInfo.quest_name;
            questSlot.target_monster_id = questInfo.target_monster_id;
            questSlot.target_monster_count = questInfo.target_monster_count;
            questSlot.target_monster_hunted = 0;
            questSlot.isClear = false;

            questSlot.questTitle.text = questInfo.quest_name + "\n" +
                "<size=26><color=#BEB5B6>" +
                MonsterInfoTableManager.GetMonsterNameFromID(questInfo.target_monster_id) + " " +
                questSlot.target_monster_hunted + " / " +
                questInfo.target_monster_count +
                "</color></size>";

            spr = Resources.LoadAll<Sprite>("Sprite/btn_color_green");
            questSlot.questButton.selfImage.sprite = spr[0];
            QuestButtonChange(questSlot, isClear);

            questSlot.gameObject.SetActive(true);
            questSlot.transform.SetParent(questContent);

            questSlots.Add(questSlot);
        }
    }

    public void DeletePlayerQuestInQuestWindow(ushort questID)
    {
        QuestSlot found = questSlots.Find(quest => quest.questID == questID);

        if (found != null)
        {
            questSlots.Remove(found);
            Destroy(found.gameObject);
        }
    }

    private void QuestListUpdate()
    {
        for (int i = 0; i < QuestManager.instance.playerQuests.Count; i++)
        {
            if (QuestManager.instance.playerQuests.TryGetValue(QuestInfoTableManager.GetQuestInfoFromQuestID(questSlots[i].questID), out PlayerQuestStateInfo state))
            {
                questSlots[i].target_monster_hunted = state.target_monster_hunted;
                questSlots[i].isClear = state.isClear;

                questSlots[i].questTitle.text = questSlots[i].quest_name + "\n" +
                    "<size=26><color=#BEB5B6>" +
                    MonsterInfoTableManager.GetMonsterNameFromID(questSlots[i].target_monster_id) + " " +
                    questSlots[i].target_monster_hunted + " / " +
                    questSlots[i].target_monster_count +
                    "</color></size>";

                // 퀘스트 성공시에만 배경 이미지 및 버튼 모양이 바뀐다
                if (questSlots[i].isClear == true)
                    QuestButtonChange(questSlots[i], state.isClear);
            }
        }
    }

    private void QuestButtonChange(QuestSlot questSlot, bool isClear)
    {
        if (isClear)
        {
            spr = Resources.LoadAll<Sprite>("Sprite/list_complete");
            questSlot.questBackground.sprite = spr[0];
            questSlot.questButton.buttonText.text = "완료";
            questSlot.questButton.buttonText.color = completeButtonTextColor;
            questSlot.questButton.selfImage.color = completeButtonColor;
        }
        else
        {
            spr = Resources.LoadAll<Sprite>("Sprite/list_default");
            questSlot.questBackground.sprite = spr[0];
            questSlot.questButton.buttonText.text = "미완료";
            questSlot.questButton.buttonText.color = defaultButtonTextColor;
            questSlot.questButton.selfImage.color = defaultButtonColor;
        }
    }
}
