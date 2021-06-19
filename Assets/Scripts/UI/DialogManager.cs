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
    [SerializeField]
    private bool isQuestDialog;
    [SerializeField]
    private bool isAwardDialog;
    [SerializeField]
    private bool isAwardEndDialog;

    private void OnEnable()
    {
        isDialogOn = true;
        currentDialogID = 0;

        // Get NPC From Player
        dialogNPC = GameManager.instance.controller.player.boundCollideNPC;

        // Turn OFF UI
        InputManager.instance.joystick.gameObject.SetActive(false);
        HUDManager.instance.combat.gameObject.SetActive(false);
        HUDManager.instance.inGame.gameObject.SetActive(false);

        // Turn ON UI
        skipBtn.gameObject.SetActive(true);

        // Initalize Npc Name
        npcName.text = dialogNPC.npc_name;

        
        // 1. ���� �ɾ��µ� NPC�� ����Ʈ�� ������..
        if (dialogNPC._isQuestExists)
        {
            // ���� �� NPC�� ����Ʈ ������
            List<QuestInfo> npcQuests = QuestInfoTableManager.questInfoList.FindAll(quest => quest.start_npc_id == dialogNPC.id);

            for (int i = 0; i < npcQuests.Count; i++)
            {
                if (QuestManager.instance.playerQuests.TryGetValue(npcQuests[i], out PlayerQuestStateInfo state))
                {
                    // ���� ����Ʈ �߿� Ŭ���� �� ����Ʈ�� �ִٸ�?
                    if (state.isClear)
                    {
                        isAwardDialog = true;

                        // ���̺��� ����Ʈ �Ϸ� ���� ��縦 ã�Ƽ� ���
                        DialogInfo awardDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].award_start_dialog_id);
                        PrintDialog(awardDialog);
                        return;
                    }

                    // �Ϸ�� ����Ʈ�� �ϳ��� ���ٸ�
                    else
                    {
                        // ���̺��� ����Ʈ �Ϸ� ��ٸ� ��縦 ã�Ƽ� ���
                        DialogInfo waitDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].wait_dialog_id);
                        PrintDialog(waitDialog);
                        return;
                    }
                }

                // ����Ʈ ����Ʈ�� �����Ƿ� NPC���� ���� ����Ʈ ����
                else
                {
                    isQuestDialog = true;

                    // �� NPC ����Ʈ�� ���� ���� ����Ʈ�� ���
                    QuestManager.instance.AddCurrentQuest(npcQuests[i]);

                    // ���� ����Ʈ�� ���� ��翡 �´� ��縦 ���̺��� ã�ƿ�
                    DialogInfo questDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == QuestManager.instance.currentQuestInfo.start_dialog_id);

                    // �� ��縦 UI�� ���
                    PrintDialog(questDialog);
                    return;
                }
            }
        }

        // 2. ���� �ɾ��µ� NPC�� ����Ʈ�� ������..
        else
        {
            // ���� �� NPC�� ����Ʈ ������
            List<QuestInfo> npcQuests = QuestInfoTableManager.questInfoList.FindAll(quest => quest.start_npc_id == dialogNPC.id);

            // NPC�� ����Ʈ�� �� �̻� ������ ���� �ʰ�, ����Ʈ ����Ʈ�� �ش� NPC�� ����Ʈ�� ���� ���
            for (int i = 0; i < npcQuests.Count; i++)
            {
                if (QuestManager.instance.playerQuests.TryGetValue(npcQuests[i], out PlayerQuestStateInfo state))
                {
                    // ���� ����Ʈ �߿� Ŭ���� �� ����Ʈ�� �ִٸ�?
                    if (state.isClear)
                    {
                        isAwardDialog = true;

                        // �� ����Ʈ�� ���� ����Ʈ�� �Ѵ�.
                        QuestManager.instance.AddCurrentQuest(npcQuests[i]);

                        // ���̺��� ����Ʈ �Ϸ� ���� ��縦 ã�Ƽ� ���
                        DialogInfo awardDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].award_start_dialog_id);
                        PrintDialog(awardDialog);
                        return;
                    }

                    // �Ϸ�� ����Ʈ�� �ϳ��� ���ٸ�
                    else
                    {
                        // ���̺��� ����Ʈ �Ϸ� ��ٸ� ��縦 ã�Ƽ� ���
                        DialogInfo waitDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].wait_dialog_id);
                        PrintDialog(waitDialog);
                        return;
                    }
                }
            }

        }

        // ���� ��Ȳ�� �ش� �Ǵ°� ������ ó������ ����Ʈ�� ���� NPC�̴�. ù��° ��縦 ����ϰԸ� �ϸ� �ȴ�.
        npcChat.text = DialogInfoTableManager.GetDialogFromDialogID(dialogNPC.dialogs[0]);
        currentDialogID = dialogNPC.dialogs[0];
        return;
    }

    private void OnDisable()
    {
        isDialogOn = false;
        isAwardDialog = false;
        isQuestDialog = false;

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
        // ����Ʈ�� �������� ��Ȳ
        if (isQuestDialog)
        {
            // ���� ���
            currentDialogID++;
            npcChat.text = DialogInfoTableManager.GetDialogFromDialogID(currentDialogID);

            // ����Ʈ�� ���������� �޾Ƶ��̱� �� ��簡 ������..
            if (currentDialogID == QuestManager.instance.currentQuestInfo.end_dialog_id)
            {
                // ��ŵ��ư�� ������, ����Ʈ ����/������ư���� ���´�.
                skipBtn.gameObject.SetActive(false);
                questBtnNode.gameObject.SetActive(true);
            }
            return;
        }

        // ����Ʈ�� �Ϸ��ؼ� ������ �ֱ� ���� ��Ȳ
        if (isAwardDialog)
        {
            // ���� ���
            currentDialogID++;
            npcChat.text = DialogInfoTableManager.GetDialogFromDialogID(currentDialogID);

            // ����Ʈ �Ϸ� �� �� ������ ����..
            if (currentDialogID == QuestManager.instance.currentQuestInfo.award_end_dialog_id)
            {
                isAwardEndDialog = true;
                return;
            }

            if (isAwardEndDialog)
            {
                // ����
                QuestManager.instance.GiveQuestAward(QuestManager.instance.currentQuestInfo);

                // ���� ����Ʈ�� ��ųʸ����� ����, ���� ����Ʈ �ʱ�ȭ
                QuestManager.instance.DeletePlayerQuest(QuestManager.instance.currentQuestInfo);

                isAwardDialog = false;
                isAwardEndDialog = false;
                return;
            }
        }

        // �� ���� ��Ȳ�� ��簡 �� ���� ��Ȳ�̹Ƿ� �ٷ� ����
        this.gameObject.SetActive(false);
    }

    public void OnQuestAccept()
    {
        QuestManager.instance.currentQuestState.isPlayerAccept = true;

        // ���������� �÷��̾��� ����Ʈ ��Ͽ��� ���
        QuestManager.instance.AddPlayerQuest(QuestManager.instance.currentQuestInfo, QuestManager.instance.currentQuestState);

        questBtnNode.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void OnQuestDecline()
    {
        // ����Ʈ�� ���������� ������ ����Ʈ������ ����
        QuestManager.instance.EmptyCurrentQuest();

        questBtnNode.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void PrintDialog(DialogInfo dialogInfo)
    {
        npcChat.text = dialogInfo.dialog;
        currentDialogID = dialogInfo.id;
    }
}
