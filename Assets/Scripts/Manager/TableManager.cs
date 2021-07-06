using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;

    [HideInInspector]
    public string playerTempInventoryPath;
    [HideInInspector]
    public string playerTempEquipmentPath;
    [HideInInspector]
    public string playerTempPath;
    [HideInInspector]
    public string playerTempQuestStatePath;

    private string weaponPath;
    private string playerPath;
    private string monsterPath;
    private string npcPath;
    private string playerInventoryPath;
    private string playerEquipmentPath;
    private string consumeItemPath;
    private string dialogPath;
    private string questPath;
    private string playerQuestStatePath;
    private string playerExpPath;
    private string playerLevelPath;
    private string sceneInfoPath;
    private string playerSpawnInfoPath;
    
    public const string FILE_EXTENSION = ".csv";

    private void Awake()
    {
        instance = this;

        // Table Path
        weaponPath = "Tables/WeaponInfo";
        playerPath = "Tables/PlayerInfo";
        monsterPath = "Tables/MonsterInfo";
        npcPath = "Tables/NPCInfo";
        playerInventoryPath = "Tables/PlayerInventory";
        playerEquipmentPath = "Tables/PlayerEquipment";
        consumeItemPath = "Tables/ConsumeItemInfo";
        dialogPath = "Tables/DialogInfo";
        questPath = "Tables/QuestInfo";
        playerQuestStatePath = "Tables/PlayerQuestState";
        playerExpPath = "Tables/PlayerExp";
        playerLevelPath = "Tables/PlayerLevel";
        sceneInfoPath = "Tables/SceneInfo";
        playerSpawnInfoPath = "Tables/PlayerSpawnInfo";

        // Temp Path
        playerTempInventoryPath = Application.persistentDataPath + "/Tables/PlayerInventory" + FILE_EXTENSION;
        playerTempEquipmentPath = Application.persistentDataPath + "/Tables/PlayerEquipment" + FILE_EXTENSION;
        playerTempPath = Application.persistentDataPath + "/Tables/PlayerInfo" + FILE_EXTENSION;
        playerTempQuestStatePath = Application.persistentDataPath + "/Tables/PlayerQuestState" + FILE_EXTENSION;

        if (!SceneInfoManager.instance.isTableManagerAwakeOnce)
        {
            WeaponInfoTableManager.LoadTable(weaponPath);
            PlayerInfoTableManager.LoadTable(playerPath);
            PlayerInventoryTableManager.LoadTable(playerInventoryPath);
            PlayerEquipmentTableManager.LoadTable(playerEquipmentPath);
            MonsterInfoTableManager.LoadTable(monsterPath);
            NPCInfoTableManager.LoadTable(npcPath);
            ConsumeInfoTableManager.LoadTable(consumeItemPath);
            DialogInfoTableManager.LoadTable(dialogPath);
            QuestInfoTableManager.LoadTable(questPath);
            PlayerQuestStateTableManager.LoadTable(playerQuestStatePath);
            PlayerExpInfoTableManager.LoadTable(playerExpPath);
            PlayerLevelInfoTableManager.LoadTable(playerLevelPath);
            SceneInfoTableManager.LoadTable(sceneInfoPath);
            PlayerSpawnInfoTableManager.LoadTable(playerSpawnInfoPath);
        }

        if (SceneInfoManager.instance.isTempDataExists)
        {
            PlayerInfoTableManager.LoadTempTable(playerTempPath);
            PlayerInventoryTableManager.LoadTempTable(playerTempInventoryPath);
            PlayerEquipmentTableManager.LoadTempTable(playerTempEquipmentPath);
            PlayerQuestStateTableManager.LoadTempTable(playerTempQuestStatePath);
        }

        SceneInfoManager.instance.isTableManagerAwakeOnce = true;
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

    public List<string> GetLinesFromTempTable(string filePath)
    {
        string line = string.Empty;
        List<string> lines = new List<string>();

        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(f, System.Text.Encoding.UTF8))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
        }

        return lines;
    }

}
