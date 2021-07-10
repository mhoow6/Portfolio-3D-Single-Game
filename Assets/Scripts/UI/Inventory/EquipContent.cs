using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipContent : MonoBehaviour
{
    public List<EquipSlot> items = new List<EquipSlot>();

    public void LoadPlayerEquipment()
    {
        // Load Inventory UI From Player Equipment
        for (int i = 0; i < PlayerInfoTableManager.playerEquipment.Length; i++)
        {
            EquipSlot newItem = items[i];

            newItem.name = "Item (" + i + ")";

            // NO DATA
            if (PlayerInfoTableManager.playerEquipment[i].item_type == (byte)ItemType.NONE)
            {
                newItem.itemIcon.sprite = null;
                newItem.itemIcon.enabled = false;
                newItem.itemCount.enabled = false;
            }
            else
                newItem.itemIcon.sprite = Resources.Load<Sprite>("Sprite/" + PlayerInfoTableManager.playerEquipment[i].icon_name);

            newItem.isEquiped = true;
            newItem.count = PlayerInfoTableManager.playerEquipment[i].count;
            newItem.itemCount.text = newItem.count.ToString();
            newItem.item_type = PlayerInfoTableManager.playerEquipment[i].item_type;
            newItem.item_id = PlayerInfoTableManager.playerEquipment[i].id;
            newItem.item_name = PlayerInfoTableManager.playerEquipment[i].icon_name;

            if (newItem.item_type == (byte)ItemType.EQUIPMENT)
                newItem.itemCount.enabled = false;

            newItem.gameObject.SetActive(true); // Disabled state -> Enable State
        }

        // Combat control Slot Item Change
        HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.sprite = Resources.Load<Sprite>("Sprite/" + PlayerInfoTableManager.playerEquipment[(int)EquipmentIndex.QUICKITEM].icon_name);
    }
}
