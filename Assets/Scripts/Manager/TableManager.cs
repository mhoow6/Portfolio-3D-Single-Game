using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : MonoBehaviour
{
    private string weaponPath;
    private string playerPath;
    private string monsterPath;
    private string npcPath;

    private void Awake()
    {
        weaponPath = Application.dataPath + "/Resources/Tables/WeaponInfo.csv";
        playerPath = Application.dataPath + "/Resources/Tables/PlayerInfo.csv";
        monsterPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";
        npcPath = Application.dataPath + "/Resources/Tables/NPCInfo.csv";

        WeaponInfoTableManager.LoadTable(weaponPath);
        PlayerInfoTableManager.LoadTable(playerPath);
        MonsterInfoTableManager.LoadTable(monsterPath);
        NPCInfoTableManager.LoadTable(npcPath);
    }

}
