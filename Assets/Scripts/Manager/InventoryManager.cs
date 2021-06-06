using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot dummyItemSlot;
    public RectTransform content;
    public List<ItemSlot> items = new List<ItemSlot>();
    public ControlSlot homeBtn;
    public bool isInventoryOn;

    // Load Inventory UI From Player Inventory
    private void Start()
    {
        for(int i = 0; i < GameManager.instance.controller.player.inventory.Length; i++)
        {
            ItemSlot newItem = Instantiate(dummyItemSlot);

            newItem.name = "Item (" + i + ")";

            // NO DATA
            if (GameManager.instance.controller.player.inventory[i].index == PlayerInventoryTableManager.EMPTY_DATA)
            {
                newItem.itemIcon.sprite = null;
                newItem.itemIcon.enabled = false;
                newItem.itemCount.gameObject.SetActive(false);
            }
            else
                newItem.itemIcon.sprite = Resources.Load<Sprite>("Sprite/" + GameManager.instance.controller.player.inventory[i].icon_name);

            newItem.itemCount.text = GameManager.instance.controller.player.inventory[i].count.ToString();
            newItem.item_type = GameManager.instance.controller.player.inventory[i].item_type;

            if (newItem.item_type == (byte)ItemType.EQUIPMENT || newItem.item_type == (byte)ItemType.NONE)
                newItem.itemCount.gameObject.SetActive(false);

            newItem.gameObject.SetActive(true);

            items.Add(newItem);
            newItem.transform.SetParent(content);
        }
    }

    private void OnEnable()
    {
        dummyItemSlot.gameObject.SetActive(false);
    }
}
