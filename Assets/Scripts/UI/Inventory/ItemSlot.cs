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

    public Image itemGradeFrame;
    public Sprite originGradeFrameSprite;
    public int originIndex;
    public Rect _rc { get => rc; }


    public Vector2 _screenPosition
    { 
        get
        {
            StartCoroutine(GetScreenPos());
            return screenPosition;
        }
    }

    public Vector2 screenPosition;

    [SerializeField]
    public RectTransform rectTransform;
    [SerializeField]
    private Rect rc;
    private const string SELECTED_FRAME = "equipment_grade_select";

    private void Awake()
    {
        originColor = itemIcon.color;
        originGradeFrameSprite = itemGradeFrame.sprite;
        pressedColor = new Color(originColor.r, originColor.g, originColor.b, ALPHA_80);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log(this.gameObject.name + ": " + _screenPosition);
    }

    // Factory Method
    public override void PointerDown(PointerEventData eventData)
    {
        // itemIcon.rectTransform.position = eventData.position;

        // Turn off EquipSlot Selected
        EquipSlot equipSelectedItem = HUDManager.instance.inventory.equipContent.items.Find(item => item.isSelected);
        if (equipSelectedItem != null && equipSelectedItem != this)
            SelectedItemOFF(equipSelectedItem);

        // Prev selected item handle
        ItemSlot selectedItem = HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected);
        if (selectedItem != null && selectedItem != this)
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

        // if item drag and drop Wrong Place OR if item drag and drop to itself
        if (eventData.pointerCurrentRaycast.gameObject.name.Substring(0, 4) != "Item" || eventData.pointerCurrentRaycast.gameObject.name == this.gameObject.name)
            this.transform.SetSiblingIndex(originIndex);

        // Slot Swap
        if (eventData.pointerCurrentRaycast.gameObject.name.Substring(0, 4) == "Item" &&
        eventData.pointerCurrentRaycast.gameObject.name != this.gameObject.name && this.item_type != (byte)ItemType.NONE)
        {
            GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
            ItemSlot targetItemSlot = HUDManager.instance.inventory.itemContent.items.Find(item => item.gameObject == targetObject);

            // Slibing Index Change
            int targetIndex = targetItemSlot.originIndex;
            this.transform.SetSiblingIndex(targetIndex); // If target Index is change first, it will be make wrong.
            targetItemSlot.transform.SetSiblingIndex(this.originIndex);

            // Change Slot in Item Slots
            ItemSlot tempItemSlot = targetItemSlot;
            HUDManager.instance.inventory.itemContent.items[targetIndex] = this;
            HUDManager.instance.inventory.itemContent.items[originIndex] = tempItemSlot;

            // Origin Index Change
            targetItemSlot.originIndex = originIndex;
            this.originIndex = targetIndex;

            Debug.Log("Swap Completed.");
        }

        // Grid Layout Rule ON
        HUDManager.instance.inventory.itemContent.layoutGroup.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemIcon.rectTransform.position = eventData.position;
        AlphaChange(pressedColor);

        if (item_type != (byte)ItemType.NONE)
        {
            // Grid Layout Rule OFF
            HUDManager.instance.inventory.itemContent.layoutGroup.enabled = false;

            // Make this First Layout
            this.transform.SetAsLastSibling();
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

    private IEnumerator GetScreenPos()
    {
        yield return new WaitForEndOfFrame();
        rectTransform.SetParent(HUDManager.instance.inventory.transform);
        screenPosition = transform.position;
        rectTransform.SetParent(HUDManager.instance.inventory.itemContent.transform);
    }
}
