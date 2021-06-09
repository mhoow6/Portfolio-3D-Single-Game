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
        StartCoroutine(InventoryButton());
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

            
        }
    }

    IEnumerator InventoryButton()
    {
        while (true)
        {
            yield return null;

            // Delete Item, Quest Item can't destroyed.
            if (HUDManager.instance.inventory.deleteBtn.isClicked)
                ItemDelete(HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected && item.item_type != (byte)ItemType.QUEST));

            // Tab Menu - All
            if (HUDManager.instance.inventory.tapMenu.tabs.Find(tab => tab.isClicked && tab.gameObject.name == "All"))
            {
                foreach (ItemSlot item in HUDManager.instance.inventory.itemContent.items)
                    item.gameObject.SetActive(true);
            }

            // Tab Menu - Weapon
            if (HUDManager.instance.inventory.tapMenu.tabs.Find(tab => tab.isClicked && tab.gameObject.name == "Weapon"))
            {
                foreach (ItemSlot item in HUDManager.instance.inventory.itemContent.items)
                {
                    if (item.item_type != (byte)ItemType.EQUIPMENT)
                        item.gameObject.SetActive(false);

                    if (item.item_type == (byte)ItemType.EQUIPMENT)
                        item.gameObject.SetActive(true);

                }
            }

            // Tab Menu - Quest
            if (HUDManager.instance.inventory.tapMenu.tabs.Find(tab => tab.isClicked && tab.gameObject.name == "Quest"))
            {
                foreach (ItemSlot item in HUDManager.instance.inventory.itemContent.items)
                {
                    if (item.item_type != (byte)ItemType.QUEST)
                        item.gameObject.SetActive(false);

                    if (item.item_type == (byte)ItemType.QUEST)
                        item.gameObject.SetActive(true);
                }
            }

            // Equip/UnEquip Item
            if (HUDManager.instance.inventory.equipBtn.isClicked)
                ItemEquip(HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected));
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

    private void ItemEquip(ItemSlot item)
    {
        if (item != null)
        {
            switch (item.item_type)
            {
                
                case (byte)ItemType.EQUIPMENT:

                    if (HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON] != null)
                    {
                        // 1. Make EquipSlot Copy
                        EquipSlot temp = Instantiate(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON]); // Deep Copy
                        temp.gameObject.SetActive(false);

                        // 2. Inventory Item to Equip Slot
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemIcon.sprite = item.itemIcon.sprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemGradeFrame.sprite = item.originGradeFrameSprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id = item.item_id;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_type = item.item_type;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemCount = item.itemCount;

                        // 3. Player's weapon object is Destroyed
                        Destroy(GameManager.instance.controller.player.weapon);

                        // 4. Now Player equips Changed Weapon
                        GameManager.instance.controller.player.weapon = GameManager.instance.controller.player.GetWeaponFromResource(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id);

                        // 5. Then Change Player Equip id
                        GameManager.instance.controller.player.equip_weapon_id = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id;

                        // 6. CopySlot To Inventory Slot
                        item.itemIcon.sprite = temp.itemIcon.sprite;
                        item.originGradeFrameSprite = temp.originGradeFrameSprite;
                        item.item_id = temp.item_id;
                        item.item_type = temp.item_type;
                        item.itemCount = temp.itemCount;

                        // 7. Destroy Temp
                        Destroy(temp);
                    }
                    break;
            }
        }
    }

    private void ItemDelete(EquipSlot item)
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
