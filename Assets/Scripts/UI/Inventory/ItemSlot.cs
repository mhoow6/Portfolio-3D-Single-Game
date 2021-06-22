using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : ControlSlot, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // ItemInfo
    public bool isSelected;
    public byte item_type;
    public ushort item_id;
    public int count;
    public byte reinforce_level;
    public string item_name;
    // ItemInfo

    public RectTransform rt;
    public Image itemGradeFrame;
    public Sprite originGradeFrameSprite;
    public int originIndex;

    private const string SELECTED_FRAME = "equipment_grade_select";
    private Rect rc;
    public Rect _rc
    {
        get
        {
            rc.x = rt.position.x - rt.rect.width * 0.5f;
            rc.y = rt.position.y + rt.rect.height * 0.5f;
            return rc;
        }
    }
    private Color dragColor;

    private void Awake()
    {
        originColor = itemIcon.color;
        originGradeFrameSprite = itemGradeFrame.sprite;
        pressedColor = itemIcon.color;
        dragColor = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, ALPHA_80);
    }

    private void Start()
    {
        // Slot's Rect
        rc.x = rt.position.x - rt.rect.width * 0.5f;
        rc.y = rt.position.y + rt.rect.height * 0.5f;

        rc.xMax = rt.rect.xMax;
        rc.yMax = rt.rect.yMax;

        rc.width = rt.rect.width;
        rc.height = rt.rect.height;
    }

    // Factory Method
    public override void PointerDown(PointerEventData eventData)
    {
        // Turn off EquipSlot Selected
        EquipSlot equipSelectedItem = HUDManager.instance.inventory.equipContent.items.Find(item => item.isSelected);
        if (equipSelectedItem != null && equipSelectedItem != this)
            SelectedItemOFF(equipSelectedItem);

        // Prev selected item handle
        ItemSlot selectedItem = HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected);
        if (selectedItem != null)
            SelectedItemOFF(selectedItem);

        // Toggle Select
        if (!isSelected && item_type != (byte)ItemType.NONE)
            itemGradeFrame.sprite = Resources.Load<Sprite>(PlayerInventoryTableManager.spritePath + SELECTED_FRAME);
        else
            itemGradeFrame.sprite = originGradeFrameSprite;

        isSelected = !isSelected;
    }

    // Factory Method
    public override void PointerUp(PointerEventData eventData)
    {
        itemIcon.rectTransform.anchoredPosition = Vector2.zero;
        ItemSlot targetItem = HUDManager.instance.inventory.itemContent.items.Find(item => item.IsInRect(eventData.position));

        // Turn off Move Icon
        HUDManager.instance.inventory.moveItemIcon.gameObject.SetActive(false);
        HUDManager.instance.inventory.moveItemIcon.sprite = null;

        // if item drag and drop Wrong Place OR if item drag and drop to itself
        if (targetItem == this || targetItem == null)
            this.transform.SetSiblingIndex(originIndex);

        // Slot Swap
        if (targetItem != null && this.item_type != (byte)ItemType.NONE)
        {
            // Slibing Index Change
            int targetIndex = targetItem.originIndex;
            this.transform.SetSiblingIndex(targetIndex); // If target Index is change first, it will be make wrong.
            targetItem.transform.SetSiblingIndex(this.originIndex);

            // Change Slot in Item Slots
            ItemSlot tempItemSlot = targetItem;
            HUDManager.instance.inventory.itemContent.items[targetIndex] = this;
            HUDManager.instance.inventory.itemContent.items[originIndex] = tempItemSlot;

            // Origin Index Change
            targetItem.originIndex = originIndex;
            this.originIndex = targetIndex;

            Debug.Log("Swap Completed.");
        }

        // Grid Layout Rule ON
        HUDManager.instance.inventory.itemContent.layoutGroup.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        AlphaChange(dragColor);

        // Put Sprite In Move Item Icon
        if (item_type != (byte)ItemType.NONE)
        {
            HUDManager.instance.inventory.moveItemIcon.gameObject.SetActive(true);
            HUDManager.instance.inventory.moveItemIcon.sprite = Resources.Load<Sprite>("Sprite/" + this.item_name);
            HUDManager.instance.inventory.moveItemIcon.rectTransform.position = eventData.position;
        }
    }

    private void AlphaChange(Color alpha)
    {
        itemIcon.color = alpha;
        itemCount.color = alpha;
    }

    private void SelectedItemOFF(ItemSlot item)
    {
        item.isSelected = false;
        item.itemGradeFrame.sprite = item.originGradeFrameSprite;
    }

    private void SelectedItemOFF(EquipSlot item)
    {
        item.isSelected = false;
        item.itemGradeFrame.sprite = item.originGradeFrameSprite;
    }

    private void OnDisable()
    {
        SelectedItemOFF(this);
    }

    public bool IsInRect(Vector2 eventPos)
    {
        if (eventPos.x >= _rc.x && eventPos.x <= _rc.x + _rc.width &&
            eventPos.y >= _rc.y - _rc.height && eventPos.y <= _rc.y)
            return true;

        return false;
    }
}
