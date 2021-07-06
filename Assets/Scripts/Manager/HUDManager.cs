using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    public CombatManager combat;
    public InventoryManager inventory;
    public MenuManager menu;
    public PlayerStateManager state;
    public SystemWindowManager system;
    public DialogManager dialog;
    public InGameInterfaceManager inGame;
    public QuestWindowManager quest;
    public DeadWindowManager dead;
    public LevelUpManager levelup;
    public LoadingManager loading;
    public AwardCheckManager awardCheck;
    public CurrentPlace place;
    public BlackFade bFade;

    public bool isUserStopUIopen
    {
        get
        {
            if (inventory.isInventoryOn || dialog.isDialogOn || quest.isQuestWindowOn || system.isSystemWindowOn || dead.isDeadWindowOn || loading.isLoadingOn || awardCheck.isAwardCheckOn || bFade.gameObject.activeSelf)
                return true;
            else
                return false;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventory.itemContent.LoadPlayerItemInventory();
        inventory.equipContent.LoadPlayerEquipment();
        quest.LoadPlayerQuest();
    }

    public void HideBottomUI()
    {
        InputManager.instance.joystick.gameObject.SetActive(false);
        combat.gameObject.SetActive(false);
        inGame.gameObject.SetActive(false);
    }

    public void ActiveUI(bool trigger)
    {
        combat.gameObject.SetActive(trigger);
        inventory.gameObject.SetActive(trigger);
        menu.gameObject.SetActive(trigger);
        state.gameObject.SetActive(trigger);
        system.gameObject.SetActive(trigger);
        quest.gameObject.SetActive(trigger);
        InputManager.instance.joystick.gameObject.SetActive(trigger);
        combat.gameObject.SetActive(trigger);
        inGame.gameObject.SetActive(trigger);
    }
}
