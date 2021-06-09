using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;

    private string weaponPath;
    private string playerPath;
    private string monsterPath;
    private string npcPath;
    private string playerInventoryPath;
    private string playerEquipmentPath;

    private void Awake()
    {
        instance = this;

        weaponPath = "Tables/WeaponInfo";
        playerPath = "Tables/PlayerInfo";
        monsterPath = "Tables/MonsterInfo";
        npcPath = "Tables/NPCInfo";
        playerInventoryPath = "Tables/PlayerInventory";
        playerEquipmentPath = "Tables/PlayerEquipment";

        WeaponInfoTableManager.LoadTable(weaponPath);
        PlayerInfoTableManager.LoadTable(playerPath);
        PlayerInventoryTableManager.LoadTable(playerInventoryPath);
        PlayerEquipmentTableManager.LoadTable(playerEquipmentPath);
        MonsterInfoTableManager.LoadTable(monsterPath);
        NPCInfoTableManager.LoadTable(npcPath);
    }

    public List<string> GetLinesFromTable(string filePath)
    {
        TextAsset txtAsset = Resources.Load<TextAsset>(filePath);

        char[] option = { '\r', '\n' };
        string[] _lines = txtAsset.text.Split(option);
        List<string> lines = new List<string>();

        foreach (string line in _lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            lines.Add(line);
        }

        return lines;
    }

}
