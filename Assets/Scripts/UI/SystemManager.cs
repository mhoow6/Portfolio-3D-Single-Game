using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public enum SoundType
{
    SFX,
    BGM
}

public class SystemManager : MonoBehaviour
{
    public SoundContent[] sounds = new SoundContent[2];
    public ControlSlot homeBtn;

    private string playerInventoryPath;
    private const string FILE_EXTENSION = ".csv";

    private void Awake()
    {
        playerInventoryPath = Application.persistentDataPath + "/Tables/PlayerInventory" + FILE_EXTENSION;
    }

    public void ExitGame() { }
    public void SaveGame() { InventorySave(); }
    public void ReturnGame() { }

    private void InventorySave()
    {
        // HUDManager.instance.inventory.itemContent.items
        // TableManager.instance.playerInventoryPath
        // TableManager.instance.playerEquipmentPath
        // index, item_type, id, icon_name, count, reinforce_lv

        string data = string.Empty;

        data = "index,item_type,id,icon_name,count,reinforce_lv";
        data += "\n";

        for (int i = 0; i < HUDManager.instance.inventory.itemContent.items.Count; i++)
        {
            data += i + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_type + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_id + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_name + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].count + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].reinforce_level;
            data += "\n";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(playerInventoryPath + FILE_EXTENSION, buffer);

        Debug.Log("인벤토리 저장 완료");
    }

    private void EquipmentSave()
    {
        string data = string.Empty;

        data = "index,item_type,id,icon_name,count,reinforce_lv";
        data += "\n";

        for (int i = 0; i < HUDManager.instance.inventory.equipContent.items.Count i++)
        {
            data += i + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_type + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_id + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].item_name + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].count + ",";
            data += HUDManager.instance.inventory.itemContent.items[i].reinforce_level;
            data += "\n";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(playerInventoryPath + FILE_EXTENSION, buffer);

        Debug.Log("장비창 저장 완료");
    }

    private void PlayerInfoSave()
    {
        string data = string.Empty;

        data = "index,item_type,id,icon_name,count,reinforce_lv";
        data += "\n";
    }
}



