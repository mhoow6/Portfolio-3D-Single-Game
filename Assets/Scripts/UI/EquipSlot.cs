using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipSlot : ControlSlot, IPointerDownHandler
{
    public TMP_Text itemCount;
    public bool isSelected;
    public byte item_type;
    public ushort item_id;

    public Image itemGradeFrame;
    public Sprite originGradeFrameSprite;

    private const string SELECTED_FRAME = "equipment_grade_select";

    private void Awake()
    {
        originColor = itemIcon.color;
        originGradeFrameSprite = itemGradeFrame.sprite;
        pressedColor = new Color(originColor.r, originColor.g, originColor.b, ALPHA_80);
    }

    public override void PointerDown(PointerEventData eventData)
    {
        // Prev selected item handle
        EquipSlot selectedItem = HUDManager.instance.inventory.equipContent.items.Find(item => item.isSelected);
        if (selectedItem != null && selectedItem != this)
        {
            selectedItem.isSelected = false;
            selectedItem.itemGradeFrame.sprite = selectedItem.originGradeFrameSprite;
        }

        // Toggle Select
        if (!isSelected && item_type != (byte)ItemType.NONE)
            itemGradeFrame.sprite = Resources.Load<Sprite>(PlayerInventoryTableManager.spritePath + SELECTED_FRAME);
        else
            itemGradeFrame.sprite = originGradeFrameSprite;

        isSelected = !isSelected;
    }
}
