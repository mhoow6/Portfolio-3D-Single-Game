using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemContent : MonoBehaviour
{
    public ItemSlot dummy;
    public GridLayoutGroup layoutGroup;
    public List<ItemSlot> items = new List<ItemSlot>();
    public TMP_Text textItemCount;

    private const float ITEM_COUNT_DURATION = 0.5f;
    private int currentItemCount;

    
    private void Start()
    {
        // Load Inventory UI From Player Inventory
        for (int i = 0; i < PlayerInventoryTableManager.playerInventory.Length; i++)
        {
            ItemSlot newItem = Instantiate(dummy);

            newItem.name = "Item (" + i + ")";

            // NO DATA
            if (PlayerInventoryTableManager.playerInventory[i].item_type == (byte)ItemType.NONE)
            {
                newItem.itemIcon.sprite = null;
                newItem.itemIcon.enabled = false;
                newItem.itemCount.gameObject.SetActive(false);
            }
            else
                newItem.itemIcon.sprite = Resources.Load<Sprite>(PlayerInventoryTableManager.spritePath + PlayerInventoryTableManager.playerInventory[i].icon_name);

            newItem.itemCount.text = PlayerInventoryTableManager.playerInventory[i].count.ToString();
            newItem.item_type = PlayerInventoryTableManager.playerInventory[i].item_type;
            newItem.item_id = PlayerInventoryTableManager.playerInventory[i].id;
            newItem.originIndex = i;

            if (newItem.item_type == (byte)ItemType.EQUIPMENT)
                newItem.itemCount.gameObject.SetActive(false);

            newItem.gameObject.SetActive(true); // Disabled state -> Enable State

            items.Add(newItem);
            newItem.transform.SetParent(this.transform);
        }

        // Dummy mess up my grid layout
        Destroy(dummy.gameObject);
        dummy = null;

        // Item Counter
        StartCoroutine(ItemCounter());
    }

    private void Update()
    {
        textItemCount.text = currentItemCount + " / 50";
    }

    IEnumerator ItemCounter()
    {
        WaitForSeconds wt = new WaitForSeconds(ITEM_COUNT_DURATION);

        while (true)
        {
            yield return wt;

            currentItemCount = items.FindAll(item => item.item_type != (byte)ItemType.NONE).Count;
        }
    }
}
