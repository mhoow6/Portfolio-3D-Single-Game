using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public VirtualJoystick joystick;
    public CustomCameraMoblie moblieCamera;

    public Vector2 moveInput;
    public Vector2 moveDelta;
    public float zoomScale; // ¹Ì±¸Çö

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(MoveInputMoblie());
            StartCoroutine(MoveDeltaMoblie());
            return;
        }

        StartCoroutine(MoveInputPC());
        StartCoroutine(MoveDeltaPC());
        StartCoroutine(ShortcutPC());
        StartCoroutine(ShortcutMenu());
    }

    IEnumerator MoveInputPC()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.inventory.isInventoryOn)
                moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    IEnumerator MoveInputMoblie()
    {
        while (true)
        {
            yield return null;

            if (joystick != null && !HUDManager.instance.inventory.isInventoryOn)
                moveInput = joystick._inputDirection;
        }
    }

    IEnumerator MoveDeltaPC()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.inventory.isInventoryOn)
                moveDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }

    IEnumerator MoveDeltaMoblie()
    {
        while (true)
        {
            yield return null;

            if (moblieCamera != null && !HUDManager.instance.inventory.isInventoryOn)
                moveDelta = moblieCamera._moveDelta;
        }
    }

    IEnumerator ShortcutPC()
    {
        while (true)
        {
            yield return null;

            // Inventory Switch
            if (Input.GetKeyDown(KeyCode.I))
            {
                bool toggle = HUDManager.instance.inventory.isInventoryOn = HUDManager.instance.inventory.isInventoryOn == false ? true : false;

                InventorySwitch(toggle);
            }
                
        }
    }

    IEnumerator ShortcutMenu()
    {
        while (true)
        {
            yield return null;

            // Inventory
            if (HUDManager.instance.menu.controlSlots.Find(slot => slot.name == "Inventory").isClicked)
            {
                Debug.Log("Inventory On");
                HUDManager.instance.inventory.isInventoryOn = true;

                InventorySwitch(HUDManager.instance.inventory.isInventoryOn);
            }
            
            if (HUDManager.instance.inventory.homeBtn.isClicked && HUDManager.instance.inventory.isInventoryOn)
            {
                Debug.Log("Inventory Off");
                HUDManager.instance.inventory.isInventoryOn = false;

                InventorySwitch(HUDManager.instance.inventory.isInventoryOn);
            }

            if (HUDManager.instance.inventory.deleteBtn.isClicked)
                ItemDelete(HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected));
        }
    }

    private void InventorySwitch(bool trigger)
    {
        HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(trigger);
        HUDManager.instance.inventory.gameObject.SetActive(trigger);
    }

    private void ItemDelete(ItemSlot item)
    {
        if (item != null)
        {
            item.itemIcon.sprite = null;
            item.itemIcon.enabled = false;
            item.itemCount.gameObject.SetActive(false);
            item.isSelected = false;
            item.itemGradeFrame.sprite = item.originGradeFrameSprite;
            item.item_type = (byte)ItemType.NONE;
            item.item_id = 0;
        }
    }
}
