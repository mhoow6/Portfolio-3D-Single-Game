using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public ItemContent itemContent;
    public TapMenu tapMenu;
    public EquipContent equipContent;
    public ControlSlot homeBtn;
    public ControlSlot deleteBtn;
    public ControlSlot equipBtn;
    public InventoryCamera inventoryCamera;
    public bool isInventoryOn;

    private void OnEnable()
    {
        isInventoryOn = true;
        HUDManager.instance.system.TimeStop();
    }

    private void OnDisable()
    {
        isInventoryOn = false;
        HUDManager.instance.system.TimeResume();
    }
}
