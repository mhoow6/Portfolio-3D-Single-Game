using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipContent : MonoBehaviour
{
    public List<EquipSlot> items = new List<EquipSlot>();

    private void Start()
    {
        // Load Inventory UI From Player Inventory
        for (int i = 0; i < PlayerEquipmentTableManager.playerEquipment.Length; i++)
        {
            EquipSlot newItem = items[i];

            newItem.name = "Item (" + i + ")";

            // NO DATA
            if (PlayerEquipmentTableManager.playerEquipment[i].item_type == (byte)ItemType.NONE)
            {
                newItem.itemIcon.sprite = null;
                newItem.itemIcon.enabled = false;
                newItem.itemCount.gameObject.SetActive(false);
            }
            else
                newItem.itemIcon.sprite = Resources.Load<Sprite>(PlayerInventoryTableManager.spritePath + PlayerEquipmentTableManager.playerEquipment[i].icon_name);

            newItem.itemCount.text = PlayerEquipmentTableManager.playerEquipment[i].count.ToString();
            newItem.item_type = PlayerEquipmentTableManager.playerEquipment[i].item_type;
            newItem.item_id = PlayerEquipmentTableManager.playerEquipment[i].id;

            if (newItem.item_type == (byte)ItemType.EQUIPMENT)
                newItem.itemCount.gameObject.SetActive(false);

            newItem.gameObject.SetActive(true); // Disabled state -> Enable State

            items.Add(newItem);
        }
    }
}
