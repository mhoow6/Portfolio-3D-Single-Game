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

    public bool bMultiClickPrevent;

    private void Awake()
    {
        instance = this;
        bMultiClickPrevent = false;
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
        StartCoroutine(ShortcutMenu()); // Moblie & PC
        StartCoroutine(InventoryButton()); // Moblie & PC
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
            if (HUDManager.instance.menu.controlSlots.Find(slot => slot.name == "Inventory").isClicked && !HUDManager.instance.inventory.isInventoryOn)
            {
                HUDManager.instance.inventory.isInventoryOn = true;
                InventorySwitch(HUDManager.instance.inventory.isInventoryOn);
            }
            
            if (HUDManager.instance.inventory.homeBtn.isClicked && HUDManager.instance.inventory.isInventoryOn)
            {
                HUDManager.instance.inventory.isInventoryOn = false;
                InventorySwitch(HUDManager.instance.inventory.isInventoryOn);
            }


            // Quick Item
            if (HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].isClicked && !bMultiClickPrevent)
            {
                bMultiClickPrevent = true;

                EquipSlot quickItem = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM];

                // Item Count Update
                quickItem.count--; // Real
                quickItem.itemCount.text = quickItem.count.ToString(); // UI

                // Get Quick Item Info
                ConsumeItemInfo quickItemInfo = ConsumeInfoTableManager.GetConsumeItemInfoFromID(quickItem.item_id);

                // Player Effect
                GameManager.instance.controller.player.currentHp += quickItemInfo.hp_heal;
                GameManager.instance.controller.player.currentMp += quickItemInfo.mp_heal;

                // Item Count 0 -> Delete
                if (quickItem.count == 0)
                    ItemDelete(quickItem);
            }
        }
    }

    IEnumerator InventoryButton()
    {
        while (true)
        {
            yield return null;

            if (!bMultiClickPrevent)
            {
                // Delete Item, Quest Item can't destroyed.
                if (HUDManager.instance.inventory.deleteBtn.isClicked)
                    ItemDelete(HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected && item.item_type != (byte)ItemType.QUEST));

                // Tab Menu - All
                if (HUDManager.instance.inventory.tapMenu.tabs.Find(tab => tab.isClicked && tab.gameObject.name == "All"))
                {
                    bMultiClickPrevent = true;

                    foreach (ItemSlot item in HUDManager.instance.inventory.itemContent.items)
                        item.gameObject.SetActive(true);
                }

                // Tab Menu - Weapon
                if (HUDManager.instance.inventory.tapMenu.tabs.Find(tab => tab.isClicked && tab.gameObject.name == "Weapon"))
                {
                    bMultiClickPrevent = true;

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
                    bMultiClickPrevent = true;

                    foreach (ItemSlot item in HUDManager.instance.inventory.itemContent.items)
                    {
                        if (item.item_type != (byte)ItemType.QUEST)
                            item.gameObject.SetActive(false);

                        if (item.item_type == (byte)ItemType.QUEST)
                            item.gameObject.SetActive(true);
                    }
                }

                // Equip Item / UnEquip Item
                if (HUDManager.instance.inventory.equipBtn.isClicked)
                {
                    bMultiClickPrevent = true;

                    ItemEquip(HUDManager.instance.inventory.itemContent.items.Find(item => item.isSelected));
                    ItemUnEquip(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM]);
                }
                    
            }
        }
    }

    private void InventorySwitch(bool trigger)
    {
        HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(trigger);
        HUDManager.instance.inventory.gameObject.SetActive(trigger);
    }

    private void ItemEquip(ItemSlot item)
    {
        bMultiClickPrevent = true;

        if (item != null)
        {
            switch (item.item_type)
            {
                case (byte)ItemType.EQUIPMENT:

                    if (HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON] != null)
                    {
                        // 1. Make EquipSlot Copy
                        EquipSlot temp = Instantiate(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON]); // Deep Copy

                        // 2. Inventory Item to Equip Slot
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].isEquiped = !HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].isEquiped;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemIcon.sprite = item.itemIcon.sprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemGradeFrame.sprite = item.originGradeFrameSprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id = item.item_id;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_type = item.item_type;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].count = item.count;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].itemCount.text = item.itemCount.text;
                        
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
                        item.count = temp.count;
                        item.itemCount.text = temp.itemCount.text;

                        // 7. Destroy Temp
                        Destroy(temp.gameObject);
                    }
                    break;

                case (byte)ItemType.CONSUME:
                    if (HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM] != null)
                    {
                        // 1. Make EquipSlot Copy
                        EquipSlot temp = Instantiate(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM]); // Deep Copy

                        // 2. Inventory Item to Equip Slot
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].isEquiped = !HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].isEquiped;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemIcon.sprite = item.itemIcon.sprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemGradeFrame.sprite = item.originGradeFrameSprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].item_id = item.item_id;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].item_type = item.item_type;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].count = item.count;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemCount.text = item.itemCount.text;

                        // 3. Combat - Quick Item Sprite Change
                        HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.sprite = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemIcon.sprite;

                        // 4. CopySlot To Inventory Slot. Empty Equip Slot doesn't need
                        if (temp.item_type != (byte)ItemType.NONE)
                        {
                            item.itemIcon.sprite = temp.itemIcon.sprite;
                            item.originGradeFrameSprite = temp.originGradeFrameSprite;
                            item.item_id = temp.item_id;
                            item.item_type = temp.item_type;
                            item.count = temp.count;
                            item.itemCount.text = temp.itemCount.text;
                        }
                        
                        // 5. Destroy Temp
                        Destroy(temp.gameObject);
                    }

                    // Equip
                    /*if (HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].item_type == (byte)ItemType.NONE)
                    {
                        // 1. Inventory Item to Equip Slot
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].isEquiped = !HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].isEquiped;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemIcon.sprite = item.itemIcon.sprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemGradeFrame.sprite = item.originGradeFrameSprite;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].item_id = item.item_id;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].item_type = item.item_type;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].count = item.count;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemCount.text = item.itemCount.text;

                        // 3. Clear Inventory Slot
                        ItemDelete(item);
                    }*/
                    
                    break;
            }
        }
    }

    private void ItemUnEquip(EquipSlot item)
    {
        // NO SELECT, NO EQUIP, EMPTY SLOT -> exit
        if (!item.isSelected && !item.isEquiped && item.item_type == (byte)ItemType.NONE)
            return;

        // Inventory already full -> exit
        if (HUDManager.instance.inventory.itemContent.currentItemCount > PlayerInventoryTableManager.MAX_SLOTS)
            return;

        // 1. Find Empty Slot
        ItemSlot emptySlot = HUDManager.instance.inventory.itemContent.items.Find(item => item.item_type == (byte)ItemType.NONE);

        // 2. Move Data To EmptySlot
        emptySlot.itemIcon.sprite = item.itemIcon.sprite;
        emptySlot.originGradeFrameSprite = item.originGradeFrameSprite;
        emptySlot.item_id = item.item_id;
        emptySlot.item_type = item.item_type;
        emptySlot.count = item.count;
        emptySlot.itemCount.text = item.itemCount.text;
        emptySlot.itemIcon.enabled = true;
        emptySlot.itemCount.enabled = true;

        // 3. Clear Equip Slot
        ItemDelete(item);
    }

    private void ItemDelete(EquipSlot item)
    {
        if (item != null)
        {
            item.itemIcon.sprite = null;
            item.itemIcon.enabled = false;
            item.itemCount.enabled = false;
            item.isSelected = false;
            item.isEquiped = false;
            item.itemGradeFrame.sprite = item.originGradeFrameSprite;
            item.item_type = (byte)ItemType.NONE;
            item.item_id = 0;
            item.count = 0;
            item.itemCount.text = "0";
        }
    }

    private void ItemDelete(ItemSlot item)
    {
        if (item != null)
        {
            item.itemIcon.sprite = null;
            item.itemIcon.enabled = false;
            item.itemCount.enabled = false;
            item.isSelected = false;
            item.itemGradeFrame.sprite = item.originGradeFrameSprite;
            item.item_type = (byte)ItemType.NONE;
            item.item_id = 0;
        }
    }
}
