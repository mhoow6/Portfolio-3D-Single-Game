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

    public bool isUserStopUIopen
    {
        get
        {
            if (inventory.isInventoryOn || dialog.isDialogOn || quest.isQuestWindowOn || system.isSystemWindowOn || dead.isDeadWindowOn)
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
}
