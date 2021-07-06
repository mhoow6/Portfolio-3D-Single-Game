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
        HUDManager.instance.HideBottomUI();

        // Turn ON UI
        skipBtn.gameObject.SetActive(true);

        // Initalize Npc Name
        npcName.text = dialogNPC.npc_name;

        
        // 1. 말을 걸었는데 NPC가 퀘스트가 있으면..
        if (dialogNPC._isQuestExists)
        {
            // 말은 건 NPC의 퀘스트 정보들
            List<QuestInfo> npcQuests = QuestInfoTableManager.questInfoList.FindAll(quest => quest.start_npc_id == dialogNPC.id);

            for (int i = 0; i < npcQuests.Count; i++)
            {
                if (QuestManager.instance.playerQuests.TryGetValue(npcQuests[i], out PlayerQuestStateInfo state))
                {
                    // 만약 퀘스트 중에 클리어 한 퀘스트가 있다면?
                    if (state.isClear)
                    {
                        isAwardDialog = true;

                        // 테이블에서 퀘스트 완료 시작 대사를 찾아서 출력
                        DialogInfo awardDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].award_start_dialog_id);
                        PrintDialog(awardDialog);
                        return;
                    }

                    // 완료된 퀘스트가 하나도 없다면
                    else
                    {
                        // 테이블에서 퀘스트 완료 기다림 대사를 찾아서 출력
                        DialogInfo waitDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].wait_dialog_id);
                        PrintDialog(waitDialog);
                        return;
                    }
                }

                // 퀘스트 리스트에 없으므로 NPC에서 받은 퀘스트 시작
                else
                {
                    isQuestDialog = true;

                    // 그 NPC 퀘스트를 현재 진행 퀘스트로 등록
                    QuestManager.instance.AddCurrentQuest(npcQuests[i]);

                    // 현재 퀘스트의 시작 대사에 맞는 대사를 테이블에서 찾아옴
                    DialogInfo questDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == QuestManager.instance.currentQuestInfo.start_dialog_id);

                    // 그 대사를 UI에 띄움
                    PrintDialog(questDialog);
                    return;
                }
            }
        }

        // 2. 말을 걸었는데 NPC가 퀘스트가 없으면..
        else
        {
            // 말은 건 NPC의 퀘스트 정보들
            List<QuestInfo> npcQuests = QuestInfoTableManager.questInfoList.FindAll(quest => quest.start_npc_id == dialogNPC.id);

            // NPC가 퀘스트를 더 이상 가지고 있지 않고, 퀘스트 리스트에 해당 NPC의 퀘스트가 있을 경우
            for (int i = 0; i < npcQuests.Count; i++)
            {
                if (QuestManager.instance.playerQuests.TryGetValue(npcQuests[i], out PlayerQuestStateInfo state))
                {
                    // 만약 퀘스트 중에 클리어 한 퀘스트가 있다면?
                    if (state.isClear)
                    {
                        isAwardDialog = true;

                        // 그 퀘스트를 현재 퀘스트로 한다.
                        QuestManager.instance.AddCurrentQuest(npcQuests[i]);

                        // 테이블에서 퀘스트 완료 시작 대사를 찾아서 출력
                        DialogInfo awardDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].award_start_dialog_id);
                        PrintDialog(awardDialog);
                        return;
                    }

                    // 완료된 퀘스트가 하나도 없다면
                    else
                    {
                        // 테이블에서 퀘스트 완료 기다림 대사를 찾아서 출력
                        DialogInfo waitDialog = DialogInfoTableManager.dialogInfoList.Find(dialog => dialog.id == npcQuests[i].wait_dialog_id);
                        PrintDialog(waitDialog);
                        return;
                    }
                }
            }

        }

        // 위의 상황에 해당 되는게 없으면 처음부터 퀘스트가 없는 NPC이다. 첫번째 대사를 출력하게만 하면 된다.
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
        // 퀘스트를 진행중인 상황
        if (isQuestDialog)
        {
            // 다음 대사
            currentDialogID++;
            npcChat.text = DialogInfoTableManager.GetDialogFromDialogID(currentDialogID);

            // 퀘스트를 최종적으로 받아들이기 전 대사가 나오면..
            if (currentDialogID == QuestManager.instance.currentQuestInfo.end_dialog_id)
            {
                // 스킵버튼이 꺼지고, 퀘스트 수락/거절버튼들이 나온다.
                skipBtn.gameObject.SetActive(false);
                questBtnNode.gameObject.SetActive(true);
            }
            return;
        }

        // 퀘스트를 완료해서 보상을 주기 위한 상황
        if (isAwardDialog)
        {
            // 다음 대사
            currentDialogID++;
            npcChat.text = DialogInfoTableManager.GetDialogFromDialogID(currentDialogID);

            // 퀘스트 완료 후 끝 대사까지 오면..
            if (currentDialogID == QuestManager.instance.currentQuestInfo.award_end_dialog_id)
            {
                isAwardEndDialog = true;
                return;
            }

            if (isAwardEndDialog)
            {
                // 보상
                QuestManager.instance.GiveQuestAward(QuestManager.instance.currentQuestInfo);

                // 현재 퀘스트를 딕셔너리에서 삭제, 현재 퀘스트 초기화
                QuestManager.instance.DeletePlayerQuest(QuestManager.instance.currentQuestInfo);

                isAwardDialog = false;
                isAwardEndDialog = false;
                this.gameObject.SetActive(false);
                return;
            }
        }

        // 그 외의 상황은 대사가 한 개인 상황이므로 바로 종료
        this.gameObject.SetActive(false);
    }

    public void OnQuestAccept()
    {
        QuestManager.instance.currentQuestState.isPlayerAccept = true;

        // 최종적으로 플레이어의 퀘스트 목록에도 등록
        QuestManager.instance.AddPlayerQuest(QuestManager.instance.currentQuestInfo, QuestManager.instance.currentQuestState);

        questBtnNode.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void OnQuestDecline()
    {
        // 퀘스트를 거절했으니 진행할 퀘스트에서도 제외
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
