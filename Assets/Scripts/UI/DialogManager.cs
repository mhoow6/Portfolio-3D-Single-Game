using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TMP_Text npcName;
    public TMP_Text npcChat;
    public Button skipBtn;
    public RectTransform questBtnNode;
    public bool isDialogOn;

    private NPC dialogNPC;
    [SerializeField]
    private ushort currentDialogID;

    private void OnEnable()
    {
        isDialogOn = true;

        // Get NPC
        dialogNPC = GameManager.instance.controller.player.boundCollideNPC;

        // Turn OFF UI
        InputManager.instance.joystick.gameObject.SetActive(false);
        HUDManager.instance.combat.gameObject.SetActive(false);
        HUDManager.instance.inGame.gameObject.SetActive(false);

        // Turn ON UI
        skipBtn.gameObject.SetActive(true);

        // Initalize Npc Name
        npcName.text = dialogNPC.npc_name;

        // Initialize text
        if (!dialogNPC._isQuestExists) // 1. NPC don't have quest
            npcChat.text = dialogNPC.dialogs[0].dialog;
        else // 2. NPC has quest
        {
            for (int i = 0; i < dialogNPC.quests.Count; i++)
            {
                if (!PlayerQuestStateTableManager.isThisQuestClear(dialogNPC.quests[i].id)) // If NPC No."i" Quest is none-cleared..
                {
                    QuestInfo currentQuestInfo = dialogNPC.quests[i]; // Get NPC Quest
                    PlayerQuestStateInfo currentQuestState = PlayerQuestStateTableManager.playerQuestStateList.Find(quest => quest.id == currentQuestInfo.id); // Get Quest State
                    currentDialogID = currentQuestInfo.start_dialog_id; // Curretn Quest Dialog

                    if (!QuestManager.instance.currentQuestState.isPlayerAccept)
                        npcChat.text = DialogInfoTableManager.GetDialogInfoFromDialogID(currentDialogID);
                    else // If current quest already accepted -> npc is waiting
                    {
                        currentDialogID = currentQuestInfo.wait_dialog_id;
                        npcChat.text = DialogInfoTableManager.GetDialogInfoFromDialogID(currentQuestInfo.wait_dialog_id);
                    }
                        

                    QuestManager.instance.currentQuestInfo = currentQuestInfo;
                    QuestManager.instance.currentQuestState = currentQuestState;
                    return;
                }
            }
        }
            
    }

    private void OnDisable()
    {
        isDialogOn = false;

        // Turn ON UI
        InputManager.instance.joystick.gameObject.SetActive(true);
        HUDManager.instance.combat.gameObject.SetActive(true);
        HUDManager.instance.inGame.gameObject.SetActive(true);

        // Empty text
        npcName.text = "";
        npcChat.text = "";

        // Null NPC
        dialogNPC = null;
    }

    public void OnSkip()
    {
        if (!dialogNPC._isQuestExists)
            this.gameObject.SetActive(false);

        else if (dialogNPC._isQuestExists && !QuestManager.instance.currentQuestState.isPlayerAccept)
        {
            // Next Dialog Ready
            currentDialogID++;
            npcChat.text = DialogInfoTableManager.GetDialogInfoFromDialogID(currentDialogID);

            // Quest Accept End Dialog -> Accept / Decline Button
            if (currentDialogID == QuestManager.instance._currentQuestInfo.end_dialog_id)
            {
                skipBtn.gameObject.SetActive(false);
                questBtnNode.gameObject.SetActive(true);
            }
                
        }

        else if (dialogNPC._isQuestExists && QuestManager.instance.currentQuestState.isPlayerAccept)
            this.gameObject.SetActive(false);
    }

    public void OnQuestAccept()
    {
        QuestManager.instance.currentQuestState.isPlayerAccept = true;
        questBtnNode.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void OnQuestDecline()
    {
        QuestManager.instance.currentQuestState.isPlayerAccept = false;
        questBtnNode.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
