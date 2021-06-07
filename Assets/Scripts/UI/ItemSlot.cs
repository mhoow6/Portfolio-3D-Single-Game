using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Image itemIcon;
    public Image itemGradeFrame;
    public TMP_Text itemCount;
    public byte item_type;
    public int originIndex;

    private const float ALPHA_80 = 0.3137255f;
    private Color originItemColor;
    private Color dragItemColor;

    private void Awake()
    {
        originItemColor = itemCount.color;
        dragItemColor = new Color(itemCount.color.r, itemCount.color.g, itemCount.color.b, ALPHA_80);
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemIcon.rectTransform.position = eventData.position;
        AlphaChange(dragItemColor);

        // Grid Layout Rule OFF
        HUDManager.instance.inventory.itemContent.layoutGroup.enabled = false;

        // Make this First Layout
        this.transform.SetAsLastSibling();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        itemIcon.rectTransform.position = eventData.position;
        AlphaChange(dragItemColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        itemIcon.rectTransform.anchoredPosition = Vector2.zero;
        AlphaChange(originItemColor);

        // Grid Layout Rule ON
        HUDManager.instance.inventory.itemContent.layoutGroup.enabled = true;

        // Slot Swap
        if (eventData.pointerCurrentRaycast.gameObject.name.Substring(0, 4) == "Item" && itemIcon.sprite != null &&
            eventData.pointerCurrentRaycast.gameObject.name != this.gameObject.name)
        {
            GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
            ItemSlot targetItemSlot = HUDManager.instance.inventory.itemContent.items.Find(item => item.gameObject == targetObject);

            // Slibing Index Change
            int targetSibingIndex = targetItemSlot.transform.GetSiblingIndex();
            targetItemSlot.transform.SetSiblingIndex(originIndex);
            this.transform.SetSiblingIndex(targetSibingIndex);

            // Change Slot in Item Slots
            ItemSlot tempItemSlot = targetItemSlot;
            HUDManager.instance.inventory.itemContent.items[targetItemSlot.originIndex] = this;
            HUDManager.instance.inventory.itemContent.items[originIndex] = tempItemSlot;

            // Origin Index Change
            int targetOriginIndex = targetItemSlot.originIndex;
            targetItemSlot.originIndex = originIndex;
            this.originIndex = targetOriginIndex;

            Debug.Log("Swap Completed.");
        }

        // Item Into Delete
        if (eventData.pointerCurrentRaycast.gameObject.name == "Delete")
        {
            // ªË¡¶
        }
        
    }

    private void AlphaChange(Color alpha)
    {
        itemIcon.color = alpha;
        itemCount.color = alpha;
    }

    private void ItemTypeCheck(ItemSlot itemSlot)
    {
        if (itemSlot.item_type == (byte)ItemType.EQUIPMENT || itemSlot.item_type == (byte)ItemType.NONE)
            itemSlot.itemCount.gameObject.SetActive(false);
        else
            itemSlot.itemCount.gameObject.SetActive(true);
    }
}
