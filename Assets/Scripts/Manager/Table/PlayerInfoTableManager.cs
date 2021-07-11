using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;

public enum EquipmentIndex
{
    WEAPON = 0,
    QUICKITEM
}

public enum ItemType
{
    NONE = 0,
    EQUIPMENT,
    CONSUME,
    QUEST,
    REINFORCE
}

public struct ItemInfo
{
    public byte item_type;
    public ushort id;
    public string icon_name;
    public int count;
    public byte reinforce_level;
}

public struct PlayerInfo
{
    public byte level;
    public float hp;
    public float mp;
    public float sp;
    public float attack_01_angle;
    public float attack_02_angle;
    public float attack_sp;
    public float combat_attack_01_angle;
    public float combat_attack_02_angle;
    public float combat_attack_03_angle;
    public float combat_attack_sp;
    public float skill_01_angle;
    public float skill_01_mp;
    public float skill_01_sp;
    public float skill_01_cooldown;
    public float skill_01_damage;
    public float skill_01_distance;
    public float skill_02_angle;
    public float skill_02_mp;
    public float skill_02_sp;
    public float skill_02_cooldown;
    public float skill_02_damage;
    public float skill_02_distance;
    public float walk_speed;
    public float run_speed;
    public float run_sp;
    public float combat_walk_speed;
    public float roll_distance;
    public float roll_sp;
    public ushort basic_weapon_id;
    public float sp_recovery_point;
    public float running_sp_reduction_rate;
    public float exp;
}

public struct PlayerTempInfo
{
    public byte level;
    public float currentHp;
    public float currentMp;
    public float currentSp;
    public float currentExp;
}

// 구조체는 읽기 전용의 데이터만 담아두자. 변경이 일어날 경우 클래스로 하는게 낫다.
public class PlayerQuestStateInfo
{
    public ushort quest_id;
    public bool isClear;
    public bool isPlayerAccept;
    public int target_monster_hunted;
}

public static class PlayerInfoTableManager
{
    public static PlayerInfo playerInfo;
    public static PlayerTempInfo playerTempInfo;
    public static ItemInfo[] playerInventory = new ItemInfo[50];
    public static ItemInfo[] playerEquipment = new ItemInfo[2];
    public static List<PlayerQuestStateInfo> playerQuests = new List<PlayerQuestStateInfo>();

    public static void LoadTable(string filePath)
    {
        string txtAsset = TableManager.instance.GetLinesWithFileStream(filePath);
        string data = txtAsset.Replace(" ", "");

        JSONNode root = JSON.Parse(data);

        // Player Info
        JSONNode player = root["Player"];
        playerInfo.level = byte.Parse(player["level"]);
        playerInfo.hp = float.Parse(player["hp"]);
        playerInfo.mp = float.Parse(player["mp"]);
        playerInfo.sp = float.Parse(player["sp"]);
        playerInfo.attack_01_angle = float.Parse(player["attack_01_angle"]);
        playerInfo.attack_02_angle = float.Parse(player["attack_02_angle"]);
        playerInfo.attack_sp = float.Parse(player["attack_sp"]);
        playerInfo.combat_attack_01_angle = float.Parse(player["combat_attack_01_angle"]);
        playerInfo.combat_attack_02_angle = float.Parse(player["combat_attack_02_angle"]);
        playerInfo.combat_attack_03_angle = float.Parse(player["combat_attack_03_angle"]);
        playerInfo.combat_attack_sp = float.Parse(player["combat_attack_sp"]);
        playerInfo.skill_01_angle = float.Parse(player["skill_01_angle"]);
        playerInfo.skill_01_mp = float.Parse(player["skill_01_mp"]);
        playerInfo.skill_01_sp = float.Parse(player["skill_01_sp"]);
        playerInfo.skill_01_cooldown = float.Parse(player["skill_01_cooldown"]);
        playerInfo.skill_01_damage = float.Parse(player["skill_01_damage"]);
        playerInfo.skill_01_distance = float.Parse(player["skill_01_distance"]);
        playerInfo.skill_02_angle = float.Parse(player["skill_02_angle"]);
        playerInfo.skill_02_mp = float.Parse(player["skill_02_mp"]);
        playerInfo.skill_02_sp = float.Parse(player["skill_02_sp"]);
        playerInfo.skill_02_cooldown = float.Parse(player["skill_02_cooldown"]);
        playerInfo.skill_02_damage = float.Parse(player["skill_02_damage"]);
        playerInfo.skill_02_distance = float.Parse(player["skill_02_distance"]);
        playerInfo.walk_speed = float.Parse(player["walk_speed"]);
        playerInfo.run_speed = float.Parse(player["run_speed"]);
        playerInfo.run_sp = float.Parse(player["run_sp"]);
        playerInfo.combat_walk_speed = float.Parse(player["combat_walk_speed"]);
        playerInfo.roll_distance = float.Parse(player["roll_distance"]);
        playerInfo.roll_sp = float.Parse(player["roll_sp"]);
        playerInfo.basic_weapon_id = ushort.Parse(player["basic_weapon_id"]);
        playerInfo.sp_recovery_point = ushort.Parse(player["sp_recovery_point"]);
        playerInfo.running_sp_reduction_rate = float.Parse(player["running_sp_reduction_rate"]);
        playerInfo.exp = float.Parse(player["exp"]);


        // Player Inventory
        JSONNode inventory = root["Inventory"];
        for (int i = 0; i < inventory.Count; i++)
        {
            ItemInfo itemInfo;

            itemInfo.item_type = byte.Parse(inventory[i]["item_type"]);
            itemInfo.id = ushort.Parse(inventory[i]["id"]);
            itemInfo.icon_name = inventory[i]["icon_name"];
            itemInfo.count = int.Parse(inventory[i]["count"]);
            itemInfo.reinforce_level = byte.Parse(inventory[i]["reinforce_level"]);

            playerInventory[i] = itemInfo;
        }

        // Player Equipment
        JSONNode equipment = root["Equipment"];
        for (int i = 0; i < equipment.Count; i++)
        {
            ItemInfo itemInfo;

            itemInfo.item_type = byte.Parse(equipment[i]["item_type"]);
            itemInfo.id = ushort.Parse(equipment[i]["id"]);
            itemInfo.icon_name = equipment[i]["icon_name"];
            itemInfo.count = int.Parse(equipment[i]["count"]);
            itemInfo.reinforce_level = byte.Parse(equipment[i]["reinforce_level"]);

            playerEquipment[i] = itemInfo;
        }


        // Player Quest State
        JSONNode queststate = root["QuestState"];
        for (int i = 0; i < queststate.Count; i++)
        {
            PlayerQuestStateInfo playerQuestInfo = new PlayerQuestStateInfo();

            playerQuestInfo.quest_id = ushort.Parse(queststate[i]["quest_id"]);
            playerQuestInfo.isClear = bool.Parse(queststate[i]["isClear"]);
            playerQuestInfo.isPlayerAccept = bool.Parse(queststate[i]["isPlayerAccept"]);
            playerQuestInfo.target_monster_hunted = int.Parse(queststate[i]["target_monster_hunted"]);

            playerQuests.Add(playerQuestInfo);
        }

    }
}
