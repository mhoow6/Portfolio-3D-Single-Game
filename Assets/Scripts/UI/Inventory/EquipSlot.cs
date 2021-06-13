using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipSlot : ControlSlot, IPointerDownHandler
{
    // ItemInfo
    public byte item_type;
    public ushort item_id;
    public ushort count;
    public byte reinforce_level;
    public string item_name;
    // ItemInfo

    public Image itemGradeFrame;
    public Sprite originGradeFrameSprite;
    public bool isSelected;
    public bool isEquiped;

    private const string SELECTED_FRAME = "equipment_grade_select";

    private void Awake()
    {
        originColor = itemIcon.color;
        originGradeFrameSprite = itemGradeFrame.sprite;
        pressedColor = new Color(originColor.r, originColor.g, originColor.b, ALPHA_80);
    }

    public override void PointerDown(PointerEventData eventData)
    {
        // Turn off ItemSlot Selected
        ItemSlot invenSelectedItem = HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected);
        if (invenSelectedItem != null && invenSelectedItem != this)
            SelectedItemOFF(invenSelectedItem);

        // Prev selected item handle
        EquipSlot selectedItem = HUDManager.instance.inventory.equipContent.items.Find(item => item.isSelected);
        if (selectedItem != null && selectedItem != this)
            SelectedItemOFF(selectedItem);

        // Toggle Select
        if (!isSelected && item_type != (byte)ItemType.NONE)
            itemGradeFrame.sprite = Resources.Load<Sprite>(PlayerInventoryTableManager.spritePath + SELECTED_FRAME);
        else
            itemGradeFrame.sprite = originGradeFrameSprite;

        isSelected = !isSelected;
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
}
