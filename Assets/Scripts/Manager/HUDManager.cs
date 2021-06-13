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
    public SystemManager system;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inventory.itemContent.LoadPlayerItemInventory();
        inventory.equipContent.LoadPlayerEquipment();
    }
}
