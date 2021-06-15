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

    public bool isSystemMenuOn;

    public void ExitGame() { }
    public void SaveGame() { InventorySave(); EquipmentSave(); PlayerInfoSave(); SceneInfoManager.instance.isTempDataExists = true; }
    public void ReturnGame() { }

    private void InventorySave()
    {
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
        File.WriteAllBytes(TableManager.instance.playerTempInventoryPath, buffer);

        Debug.Log("인벤토리 저장 완료");
    }

    private void EquipmentSave()
    {
        string data = string.Empty;

        data = "index,item_type,id,icon_name,count,reinforce_lv";
        data += "\n";

        for (int i = 0; i < HUDManager.instance.inventory.equipContent.items.Count; i++)
        {
            data += i + ",";
            data += HUDManager.instance.inventory.equipContent.items[i].item_type + ",";
            data += HUDManager.instance.inventory.equipContent.items[i].item_id + ",";
            data += HUDManager.instance.inventory.equipContent.items[i].item_name + ",";
            data += HUDManager.instance.inventory.equipContent.items[i].count + ",";
            data += HUDManager.instance.inventory.equipContent.items[i].reinforce_level;
            data += "\n";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(TableManager.instance.playerTempEquipmentPath, buffer);

        Debug.Log("장비창 저장 완료");
    }

    private void PlayerInfoSave()
    {
        string data = string.Empty;

        data = "level,hp,mp,sp,currentHp,currentMp,currentSp";
        data += "\n";

        data += GameManager.instance.controller.player.level + ",";
        data += GameManager.instance.controller.player.hp + ",";
        data += GameManager.instance.controller.player.mp + ",";
        data += GameManager.instance.controller.player.sp + ",";
        data += GameManager.instance.controller.player.currentHp + ",";
        data += GameManager.instance.controller.player.currentMp + ",";
        data += GameManager.instance.controller.player.currentSp + ",";

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(TableManager.instance.playerTempPath, buffer);

        Debug.Log("플레이어 임시정보 저장 완료");
    }
}



