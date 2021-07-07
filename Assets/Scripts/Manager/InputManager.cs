using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public VirtualJoystick joystick;
    public CustomCameraMoblie moblieCamera;

    public Vector2 moveInput
    {
        get
        {
            if (keyboardMove.magnitude == 0)
                return joystickMove;

            if (joystickMove.magnitude == 0)
                return keyboardMove;

            return Vector2.zero;
        }
    }

    private Vector2 keyboardMove;
    private Vector2 joystickMove;

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
        StartCoroutine(ShortcutMenu()); // Moblie & PC
        StartCoroutine(InventoryButton()); // Moblie & PC
        StartCoroutine(SystemButton()); // Moblie & PC
        StartCoroutine(QuestButton()); // Moblie & PC

        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(MoveInputMoblie());
            StartCoroutine(MoveDeltaMoblie());
            return;
        }

        StartCoroutine(MoveInputPC());
        StartCoroutine(MoveInputMoblie());
        StartCoroutine(MoveDeltaPC());
        StartCoroutine(ShortcutPC());
    }

    IEnumerator MoveInputPC()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.isUserStopUIopen)
                keyboardMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            else
                keyboardMove = Vector2.zero;
        }
    }

    IEnumerator MoveInputMoblie()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.isUserStopUIopen)
                joystickMove = joystick._inputDirection;
            else
                joystickMove = Vector2.zero;
        }
    }

    IEnumerator MoveDeltaPC()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.isUserStopUIopen)
                moveDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            else
                moveDelta = Vector2.zero;
        }
    }

    IEnumerator MoveDeltaMoblie()
    {
        while (true)
        {
            yield return null;

            if (!HUDManager.instance.isUserStopUIopen)
                moveDelta = moblieCamera._moveDelta;
            else
                moveDelta = Vector2.zero;
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

            // Quick Item
            if (Input.GetKeyDown(KeyCode.Z) && !HUDManager.instance.inventory.isInventoryOn)
                UseQuickItem();

            // System Window
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bool toggle = HUDManager.instance.system.isSystemWindowOn = HUDManager.instance.system.isSystemWindowOn == false ? true : false;
                SystemWindowSwitch(toggle);
            }

            // Dialog NPC
            if (Input.GetKeyDown(KeyCode.F) && GameManager.instance.controller.player.boundCollideNPC != null)
            {
                DialogSwitch(true);
            }

            // Quest Window
            if (Input.GetKeyDown(KeyCode.J))
            {
                bool toggle = HUDManager.instance.system.isSystemWindowOn = HUDManager.instance.system.isSystemWindowOn == false ? true : false;
                QuestWindowSwitch(toggle);
            }

        }
    }

    IEnumerator ShortcutMenu()
    {
        while (true)
        {
            yield return null;

            // Inventory
            if (HUDManager.instance.menu.controlSlots[(int)MenuIndex.INVENTORY].isClicked && !HUDManager.instance.inventory.isInventoryOn)
            {
                InventorySwitch(true);
            }

            // System Window
            if (HUDManager.instance.menu.controlSlots[(int)MenuIndex.SYSTEM].isClicked && !HUDManager.instance.system.isSystemWindowOn)
            {
                SystemWindowSwitch(true);
            }

            // Quick Item
            if (HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].isClicked && !bMultiClickPrevent)
            {
                bMultiClickPrevent = true;
                UseQuickItem();
            }

            // Quest Window
            if (HUDManager.instance.menu.controlSlots[(int)MenuIndex.QUEST].isClicked && !HUDManager.instance.quest.isQuestWindowOn)
            {
                QuestWindowSwitch(true);
            }
        }
    }

    IEnumerator SystemButton()
    {
        while (true)
        {
            yield return null;

            // Home Btn
            if (HUDManager.instance.system.homeBtn.isClicked && HUDManager.instance.system.isSystemWindowOn)
            {
                SystemWindowSwitch(false);
            }

            // Pause, Save, Quit -> OnClick()
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

                // Home Button
                if (HUDManager.instance.inventory.homeBtn.isClicked && HUDManager.instance.inventory.isInventoryOn)
                {
                    InventorySwitch(false);
                }

            }
        }
    }

    IEnumerator QuestButton()
    {
        while (true)
        {
            yield return null;

            // Home Btn
            if (HUDManager.instance.quest.homebtn.isClicked && HUDManager.instance.quest.isQuestWindowOn)
                QuestWindowSwitch(false);
        }
    }

    private void InventorySwitch(bool trigger)
    {
        HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(trigger);
        HUDManager.instance.inventory.gameObject.SetActive(trigger);
    }

    private void SystemWindowSwitch(bool trigger)
    {
        HUDManager.instance.system.gameObject.SetActive(trigger);
    }

    private void QuestWindowSwitch(bool trigger)
    {
        HUDManager.instance.quest.gameObject.SetActive(trigger);
    }

    private void DialogSwitch(bool trigger)
    {
        HUDManager.instance.dialog.gameObject.SetActive(trigger);
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
                        //  2.1. bools
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].isEquiped = true;
                        item.isSelected = false;

                        //  2.2. ItemSlot Properties -> EquipSlot Properties
                        ItemValueMove(item, HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON]);
                        
                        // 3. Player's weapon object is Destroyed
                        Destroy(GameManager.instance.controller.player.weapon);

                        // 4. Now Player equips Changed Weapon
                        GameManager.instance.controller.player.weapon = GameManager.instance.controller.player.GetWeaponFromResource(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id);

                        // 5. Then Change Player Equip id
                        GameManager.instance.controller.player.equip_weapon_id = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.WEAPON].item_id;

                        // 6. CopySlot To Inventory Slot
                        ItemValueMove(temp, item);

                        // 7. Destroy Temp
                        Destroy(temp.gameObject);
                    }
                    break;

                case (byte)ItemType.CONSUME:
                    if (HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM] != null)
                    {
                        // 1. Make EquipSlot Copy
                        EquipSlot temp = Instantiate(HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM]); // Deep Copy;

                        // 2. Inventory Item to Equip Slot
                        //  2.1. bools
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].isEquiped = true;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemIcon.enabled = true;
                        HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemCount.enabled = true;
                        item.isSelected = false;

                        //  2.2. ItemSlot Properties -> EquipSlot Properties
                        ItemValueMove(item, HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM]);

                        // 3. Combat - Quick Item Sprite Change
                        HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.enabled = true;
                        HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.sprite = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM].itemIcon.sprite;

                        // 4. CopySlot To Inventory Slot
                        if (temp.item_type == (byte)ItemType.NONE)
                        {
                            item.itemIcon.enabled = false;
                            item.itemCount.enabled = false;
                        }
                        ItemValueMove(temp, item);

                        // 5. Destroy Temp
                        Destroy(temp.gameObject);
                    }
                    break;
            }
        }
    }

    private void ItemUnEquip(EquipSlot item)
    {
        // NO SELECT, NO EQUIP, EMPTY SLOT -> exit
        if (!item.isSelected || !item.isEquiped || item.item_type == (byte)ItemType.NONE)
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

        // 3. Combat - Quick Item Sprite OFF
        HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.enabled = false;

        // 4. Clear Equip Slot
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

    private void ItemValueMove(ItemSlot send, EquipSlot receive)
    {
        receive.itemIcon.sprite = send.itemIcon.sprite;
        receive.itemGradeFrame.sprite = send.originGradeFrameSprite;
        receive.item_id = send.item_id;
        receive.item_type = send.item_type;
        receive.count = send.count;
        receive.itemCount.text = send.itemCount.text;
        receive.reinforce_level = send.reinforce_level;
        receive.item_name = send.item_name;
    }

    private void ItemValueMove(EquipSlot send, ItemSlot receive)
    {
        receive.itemIcon.sprite = send.itemIcon.sprite;
        receive.itemGradeFrame.sprite = send.originGradeFrameSprite;
        receive.item_id = send.item_id;
        receive.item_type = send.item_type;
        receive.count = send.count;
        receive.itemCount.text = send.itemCount.text;
        receive.reinforce_level = send.reinforce_level;
        receive.item_name = send.item_name;
    }

    private void UseQuickItem()
    {
        EquipSlot quickItem = HUDManager.instance.inventory.equipContent.items[(int)EquipmentIndex.QUICKITEM];

        // 1. Item Count Update
        quickItem.count--; // Real
        quickItem.itemCount.text = quickItem.count.ToString(); // UI

        // 2. Get Quick Item Info
        ConsumeItemInfo quickItemInfo = ConsumeInfoTableManager.GetConsumeItemInfoFromID(quickItem.item_id);

        // 3. Player get healed, NO Overflow
        if (GameManager.instance.controller.player.currentHp + quickItemInfo.hp_heal > GameManager.instance.controller.player.hp)
            GameManager.instance.controller.player.currentHp = GameManager.instance.controller.player.hp;
        else
            GameManager.instance.controller.player.currentHp += quickItemInfo.hp_heal;

        if (GameManager.instance.controller.player.currentMp + quickItemInfo.mp_heal > GameManager.instance.controller.player.mp)
            GameManager.instance.controller.player.currentMp = GameManager.instance.controller.player.mp;
        else
            GameManager.instance.controller.player.currentMp += quickItemInfo.mp_heal;

        // 4. Item Count 0 -> Delete
        if (quickItem.count == 0)
        {
            HUDManager.instance.combat.controlSlots[(int)CombatIndex.QUICKITEM].itemIcon.enabled = false;
            ItemDelete(quickItem);
        }
    }
}
