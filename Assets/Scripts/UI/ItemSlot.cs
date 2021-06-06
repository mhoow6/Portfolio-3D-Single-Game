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

        // Swap
        if (eventData.pointerCurrentRaycast.gameObject.name.Substring(0, 4) == "Item")
        {
            Debug.Log("Item Confirmed.");
            if (eventData.pointerCurrentRaycast.gameObject.name != this.gameObject.name)
            {
                GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
                ItemSlot targetItemSlot = HUDManager.instance.inventory.items.Find(item => item.gameObject == targetObject);

                // Temp
                ItemSlot temp = Instantiate(this); // 정말 필요할까..?
                temp.gameObject.SetActive(false);

                // Data Swap
                this.itemIcon.sprite = targetItemSlot.itemIcon.sprite;
                this.itemGradeFrame.sprite = targetItemSlot.itemGradeFrame.sprite;
                this.itemCount.text = targetItemSlot.itemCount.text;
                this.item_type = targetItemSlot.item_type;

                targetItemSlot.itemIcon.sprite = temp.itemIcon.sprite;
                targetItemSlot.itemGradeFrame.sprite = temp.itemGradeFrame.sprite;
                targetItemSlot.itemCount.text = temp.itemCount.text;
                targetItemSlot.item_type = temp.item_type;

                Destroy(temp.gameObject); // 정말 필요할까..?

                // Both Item Type Check
                if (this.item_type == (byte)ItemType.EQUIPMENT || this.item_type == (byte)ItemType.NONE)
                    this.itemCount.gameObject.SetActive(false);
                else
                    this.itemCount.gameObject.SetActive(true);
                if (targetItemSlot.item_type == (byte)ItemType.EQUIPMENT || targetItemSlot.item_type == (byte)ItemType.NONE)
                    targetItemSlot.itemCount.gameObject.SetActive(false);
                else
                    targetItemSlot.itemCount.gameObject.SetActive(true);

                // After Swap With Empty Slot
                if (this.itemIcon.sprite == null)
                {
                    this.itemIcon.enabled = false;
                    targetItemSlot.itemIcon.enabled = true;
                }
                    
                
                Debug.Log("Swap Completed.");
            }
        }

        // Delete
        if (eventData.pointerCurrentRaycast.gameObject.name == "Delete")
        {
            // 삭제 메시지 팝업
        }
        
    }

    private void AlphaChange(Color alpha)
    {
        itemIcon.color = alpha;
        itemCount.color = alpha;
    }
}
