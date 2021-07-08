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

public class SystemWindowManager : MonoBehaviour
{
    public SoundContent[] sounds = new SoundContent[2];
    public ControlSlot homeBtn;

    public bool isSystemWindowOn;

    public void ExitGame() { Application.Quit(); }

    public void SaveGame()
    {
        string data = string.Empty;

        // PlayerInfo
        data +=
            "{\n" +
            "\t" + "\"Player\"" + ": {\n" +
            "\t\t" + "\"level\" : " + GameManager.instance.controller.player.level + "," + "\n";

        if (GameManager.instance.controller.player.currentHp != 0)
        {
            data +=
                "\t\t" + "\"hp\" : " + GameManager.instance.controller.player.currentHp + "," + "\n" +
                "\t\t" + "\"mp\" : " + GameManager.instance.controller.player.currentMp + "," + "\n" +
                "\t\t" + "\"sp\" : " + GameManager.instance.controller.player.currentSp + "," + "\n";
        }
        else
        {
            data +=
                "\t\t" + "\"hp\" : " + PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).hp + "," + "\n" +
                "\t\t" + "\"mp\" : " + PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).mp + "," + "\n" +
                "\t\t" + "\"sp\" : " + PlayerLevelInfoTableManager.GetPlayerLevelInfoFromLevel(GameManager.instance.controller.player.level).sp + "," + "\n";
        }

        data += 
            "\t\t" + "\"attack_01_angle\" : " + PlayerInfoTableManager.playerInfo.attack_01_angle + "," + "\n" +
            "\t\t" + "\"attack_02_angle\" : " + PlayerInfoTableManager.playerInfo.attack_02_angle + "," + "\n" +
            "\t\t" + "\"attack_sp\" : " + PlayerInfoTableManager.playerInfo.attack_sp + "," + "\n" +
            "\t\t" + "\"combat_attack_01_angle\" : " + PlayerInfoTableManager.playerInfo.combat_attack_01_angle + "," + "\n" +
            "\t\t" + "\"combat_attack_02_angle\" : " + PlayerInfoTableManager.playerInfo.combat_attack_02_angle + "," + "\n" +
            "\t\t" + "\"combat_attack_03_angle\" : " + PlayerInfoTableManager.playerInfo.combat_attack_03_angle + "," + "\n" +
            "\t\t" + "\"combat_attack_sp\" : " + PlayerInfoTableManager.playerInfo.combat_attack_sp + "," + "\n" +
            "\t\t" + "\"skill_01_angle\" : " + PlayerInfoTableManager.playerInfo.skill_01_angle + "," + "\n" +
            "\t\t" + "\"skill_01_mp\" : " + PlayerInfoTableManager.playerInfo.skill_01_mp + "," + "\n" +
            "\t\t" + "\"skill_01_sp\" : " + PlayerInfoTableManager.playerInfo.skill_01_sp + "," + "\n" +
            "\t\t" + "\"skill_01_cooldown\" : " + PlayerInfoTableManager.playerInfo.skill_01_cooldown + "," + "\n" +
            "\t\t" + "\"skill_01_damage\" : " + PlayerInfoTableManager.playerInfo.skill_01_damage + "," + "\n" +
            "\t\t" + "\"skill_01_distance\" : " + PlayerInfoTableManager.playerInfo.skill_01_distance + "," + "\n" +
            "\t\t" + "\"skill_02_angle\" : " + PlayerInfoTableManager.playerInfo.skill_02_angle + "," + "\n" +
            "\t\t" + "\"skill_02_mp\" : " + PlayerInfoTableManager.playerInfo.skill_02_mp + "," + "\n" +
            "\t\t" + "\"skill_02_sp\" : " + PlayerInfoTableManager.playerInfo.skill_02_sp + "," + "\n" +
            "\t\t" + "\"skill_02_cooldown\" : " + PlayerInfoTableManager.playerInfo.skill_02_cooldown + "," + "\n" +
            "\t\t" + "\"skill_02_damage\" : " + PlayerInfoTableManager.playerInfo.skill_02_damage + "," + "\n" +
            "\t\t" + "\"skill_02_distance\" : " + PlayerInfoTableManager.playerInfo.skill_02_distance + "," + "\n" +
            "\t\t" + "\"walk_speed\" : " + PlayerInfoTableManager.playerInfo.walk_speed + "," + "\n" +
            "\t\t" + "\"run_speed\" : " + PlayerInfoTableManager.playerInfo.run_speed + "," + "\n" +
            "\t\t" + "\"run_sp\" : " + PlayerInfoTableManager.playerInfo.run_sp + "," + "\n" +
            "\t\t" + "\"combat_walk_speed\" : " + PlayerInfoTableManager.playerInfo.combat_walk_speed + "," + "\n" +
            "\t\t" + "\"roll_distance\" : " + PlayerInfoTableManager.playerInfo.roll_distance + "," + "\n" +
            "\t\t" + "\"roll_sp\" : " + PlayerInfoTableManager.playerInfo.roll_sp + "," + "\n" +
            "\t\t" + "\"basic_weapon_id\" : " + PlayerInfoTableManager.playerInfo.basic_weapon_id + "," + "\n" +
            "\t\t" + "\"sp_recovery_point\" : " + PlayerInfoTableManager.playerInfo.sp_recovery_point + "," + "\n" +
            "\t\t" + "\"running_sp_reduction_rate\" : " + PlayerInfoTableManager.playerInfo.running_sp_reduction_rate + "," + "\n" +
            "\t\t" + "\"exp\" : " + PlayerInfoTableManager.playerInfo.exp + "\n" +
            "\t" + "}" + "," + "\n";


        // PlayerInventory
        data +=
            "\t" + "\"Inventory\"" + ": {\n";

        for (int i = 0; i < HUDManager.instance.inventory.itemContent.items.Count; i++)
        {
            data +=
                "\t\t" + "\"" + i + "\"" + " : {" + "\n" +
                "\t\t\t" + "\"item_type\" : " + HUDManager.instance.inventory.itemContent.items[i].item_type + "," + "\n" +
                "\t\t\t" + "\"id\" : " + HUDManager.instance.inventory.itemContent.items[i].item_id + "," + "\n" +
                "\t\t\t" + "\"icon_name\" : " + "\"" + HUDManager.instance.inventory.itemContent.items[i].item_name + "\"" + "," + "\n" +
                "\t\t\t" + "\"count\" : " + HUDManager.instance.inventory.itemContent.items[i].count + "," + "\n" +
                "\t\t\t" + "\"reinforce_level\" : " + HUDManager.instance.inventory.itemContent.items[i].reinforce_level + "\n" +
                "\t\t" + "}";

            if (i != HUDManager.instance.inventory.itemContent.items.Count - 1)
                data += "," + "\n";
            else
                data += "\n";
        }
        data += "\t" + "}" + "," + "\n";


        // PlayerEquipment
        data +=
            "\t" + "\"Equipment\"" + ": {\n";

        for (int i = 0; i < HUDManager.instance.inventory.equipContent.items.Count; i++)
        {
            data +=
                "\t\t" + "\"" + i + "\"" + " : {" + "\n" +
                "\t\t\t" + "\"item_type\" : " + HUDManager.instance.inventory.equipContent.items[i].item_type + "," + "\n" +
                "\t\t\t" + "\"id\" : " + HUDManager.instance.inventory.equipContent.items[i].item_id + "," + "\n" +
                "\t\t\t" + "\"icon_name\" : " + "\"" + HUDManager.instance.inventory.equipContent.items[i].item_name + "\"" + "," + "\n" +
                "\t\t\t" + "\"count\" : " + HUDManager.instance.inventory.equipContent.items[i].count + "," + "\n" +
                "\t\t\t" + "\"reinforce_level\" : " + HUDManager.instance.inventory.equipContent.items[i].reinforce_level + "\n" +
                "\t\t" + "}";

            if (i != HUDManager.instance.inventory.equipContent.items.Count - 1)
                data += "," + "\n";
            else
                data += "\n";
        }
        data += "\t" + "}" + "," + "\n";


        // PlayerQuestState
        data +=
            "\t" + "\"QuestState\"" + ": {\n";


        int count = 0;
        foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in QuestManager.instance.playerQuests)
        {
            count++;

            data +=
                "\t\t" + "\"" + count + "\"" + " : {" + "\n" +
                "\t\t\t" + "\"quest_id\" : " + quests.Key.id + "," + "\n" +
                "\t\t\t" + "\"isClear\" : " + quests.Value.isClear + "," + "\n" +
                "\t\t\t" + "\"isPlayerAccept\" : " + quests.Value.isPlayerAccept + "," + "\n" +
                "\t\t\t" + "\"target_monster_hunted\" : " + quests.Value.target_monster_hunted + "," + "\n" +
                "\t\t" + "}";

            if (count != QuestManager.instance.playerQuests.Count - 1)
                data += "," + "\n";
            else
                data += "\n";
        }
        count = 0;
        data += "\t" + "}" + "\n";

        data += "}";

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(TableManager.instance._playerPath, buffer);
    }

    public void ReturnGame() { this.gameObject.SetActive(false); }

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
        //File.WriteAllBytes(TableManager.instance.playerTempInventoryPath, buffer);

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
        //File.WriteAllBytes(TableManager.instance.playerTempEquipmentPath, buffer);

        Debug.Log("장비창 저장 완료");
    }

    private void PlayerInfoSave()
    {
        string data = string.Empty;

        data = "level,currentHp,currentMp,currentSp,currentExp";
        data += "\n";

        data += GameManager.instance.controller.player.level + ",";

        if (GameManager.instance.controller.player.currentHp != 0)
            data += GameManager.instance.controller.player.currentHp + ",";

        if (GameManager.instance.controller.player.currentHp == 0)
            data += GameManager.instance.controller.player.hp + ",";

        data += GameManager.instance.controller.player.currentMp + ",";
        data += GameManager.instance.controller.player.currentSp + ",";
        data += GameManager.instance.controller.player.currentExp;

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        //File.WriteAllBytes(TableManager.instance.playerTempPath, buffer);

        Debug.Log("플레이어 임시정보 저장 완료");
    }

    private void PlayerQuestSave()
    {
        string data = string.Empty;

        data = "id,isClear,isPlayerAccept,target_monster_hunted";
        data += "\n";

        foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in QuestManager.instance.playerQuests)
        {
            data += quests.Key.id + ",";
            data += quests.Value.isClear + ",";
            data += quests.Value.isPlayerAccept + ",";
            data += quests.Value.target_monster_hunted;
            data += "\n";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        //File.WriteAllBytes(TableManager.instance.playerTempQuestStatePath, buffer);

        Debug.Log("플레이어 퀘스트 정보 저장 완료");
    }

    private void OnEnable()
    {
        isSystemWindowOn = true;
        TimeStop();
    }

    private void OnDisable()
    {
        isSystemWindowOn = false;
        TimeResume();
    }

    public void TimeStop()
    {
        Time.timeScale = 0;
    }

    public void TimeResume()
    {
        Time.timeScale = 1f;
    }
}



