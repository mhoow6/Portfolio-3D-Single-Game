using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : MonoBehaviour
{
    public static TableManager instance;

    private string weaponPath;
    public string _playerPath { get => playerPath; }
    private string playerPath;
    private string monsterPath;
    private string npcPath;
    private string consumeItemPath;
    private string dialogPath;
    private string questPath;
    private string playerExpPath;
    private string playerLevelPath;
    private string sceneInfoPath;
    private string playerSpawnInfoPath;

    public string _FILE_EXTENSION { get => FILE_EXTENSION; }
    private const string FILE_EXTENSION = ".csv";
    public string _JSON_EXTENSION { get => JSON_EXTENSION; }
    private const string JSON_EXTENSION = ".json";

    private void Awake()
    {
        instance = this;

        // Data Path
        weaponPath = Application.persistentDataPath + "/Xmls/WeaponInfo.xml";
        playerPath = Application.persistentDataPath + "/Jsons/Player.json";
        monsterPath = Application.persistentDataPath + "/Tables/MonsterInfo.csv";
        npcPath = Application.persistentDataPath + "/Tables/NPCInfo.csv";
        consumeItemPath = Application.persistentDataPath + "/Xmls/ConsumeItemInfo.xml";
        dialogPath = Application.persistentDataPath + "/Tables/DialogInfo.csv";
        questPath = Application.persistentDataPath + "/Tables/QuestInfo.csv";
        playerExpPath = Application.persistentDataPath + "/Tables/PlayerExp.csv";
        playerLevelPath = Application.persistentDataPath + "/Tables/PlayerLevel.csv";
        sceneInfoPath = Application.persistentDataPath + "/Tables/SceneInfo.csv";
        playerSpawnInfoPath = Application.persistentDataPath + "/Tables/PlayerSpawnInfo.csv";

        PlayerInfoTableManager.LoadTable(playerPath);

        if (!SceneInfoManager.instance.isTableManagerAwakeOnce)
        {
            DirectoryCheck(Application.persistentDataPath + "/Tables");
            DirectoryCheck(Application.persistentDataPath + "/Jsons");
            DirectoryCheck(Application.persistentDataPath + "/Xmls");

            WeaponInfoTableManager.LoadTable(weaponPath);
            MonsterInfoTableManager.LoadTable(monsterPath);
            NPCInfoTableManager.LoadTable(npcPath);
            ConsumeInfoTableManager.LoadTable(consumeItemPath);
            DialogInfoTableManager.LoadTable(dialogPath);
            QuestInfoTableManager.LoadTable(questPath);
            PlayerExpInfoTableManager.LoadTable(playerExpPath);
            PlayerLevelInfoTableManager.LoadTable(playerLevelPath);
            SceneInfoTableManager.LoadTable(sceneInfoPath);
            PlayerSpawnInfoTableManager.LoadTable(playerSpawnInfoPath);
        }

        SceneInfoManager.instance.isTableManagerAwakeOnce = true;
    }

    public List<string> GetLinesFromTableTextAsset(string filePath)
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

    public List<string> GetLinesFromTableFileStream(string filePath)
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

    public string GetLinesWithFileStream(string filePath)
    {
        string line = string.Empty;
        string lines = string.Empty;

        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(f, System.Text.Encoding.UTF8))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    lines += line;
                }
            }
        }

        return lines;
    }

    public void DirectoryCheck(string directory)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

}
