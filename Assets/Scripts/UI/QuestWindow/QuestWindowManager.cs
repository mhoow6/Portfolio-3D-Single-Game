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
        QuestListUpdate();
    }

    private void OnDisable()
    {
        isQuestWindowOn = false;
    }

    public void LoadPlayerQuest()
    {
        if (PlayerQuestStateTableManager.playerQuestStateList.Count == 0)
            return;

        for (int i = 0; i < PlayerQuestStateTableManager.playerQuestStateList.Count; i++)
            AddPlayerQuestInQuestWindow(PlayerQuestStateTableManager.playerQuestStateList[i].quest_id, PlayerQuestStateTableManager.playerQuestStateList[i].isClear);
    }

    public void AddPlayerQuestInQuestWindow(ushort questID, bool isClear)
    {
        QuestSlot questSlot = Instantiate(questDummy); // 오브젝트 풀링
        QuestInfo questInfo = QuestInfoTableManager.GetQuestInfoFromQuestID(questID);

        questSlot.name = "Quest";
        questSlot.questID = questID;
        questSlot.quest_name = questInfo.quest_name;
        questSlot.target_monster_id = questInfo.target_monster_id;
        questSlot.target_monster_count = questInfo.target_monster_count;
        questSlot.isClear = false;

        spr = Resources.LoadAll<Sprite>("Sprite/btn_color_green");
        questSlot.questButton.selfImage.sprite = spr[0];

        QuestButtonChange(questSlot, isClear);

        questSlots.Add(questSlot);
        questSlot.questTitle.text = questInfo.quest_name + "\n" +
            "<size=26><color=#BEB5B6>" +
            MonsterInfoTableManager.GetMonsterNameFromID(questSlot.target_monster_id) +
            " 0 / " +
            questSlot.target_monster_count +
            "</color></size>";
        questSlot.gameObject.SetActive(true);
        questSlot.transform.SetParent(questContent);
    }

    public void DeletePlayerQuestInQuestWindow(ushort questID)
    {
        QuestSlot found = questSlots.Find(quest => quest.questID == questID);
        questSlots.Remove(found);
        found.gameObject.SetActive(false);
    }

    private void QuestListUpdate()
    {
        for (int i = 0; i < questSlots.Count; i++)
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
