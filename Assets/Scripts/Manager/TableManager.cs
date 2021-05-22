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

    private void Awake()
    {
        instance = this;
        weaponPath = Application.dataPath + "/Resources/Tables/WeaponInfo.csv";
        playerPath = Application.dataPath + "/Resources/Tables/PlayerInfo.csv";
        monsterPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";
        npcPath = Application.dataPath + "/Resources/Tables/NPCInfo.csv";

        if (!SceneInfoManager.isTableManagerAwakeOnce)
        {
            WeaponInfoTableManager.LoadTable(weaponPath);
            PlayerInfoTableManager.LoadTable(playerPath);
            MonsterInfoTableManager.LoadTable(monsterPath);
            NPCInfoTableManager.LoadTable(npcPath);
            SceneInfoManager.isTableManagerAwakeOnce = true;
        }
    }

}
