using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 테이블로 부터 데이터 로드
public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
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

        WeaponInfoTableManager.LoadTable(weaponPath);
        PlayerInfoTableManager.LoadTable(playerPath);
        MonsterInfoTableManager.LoadTable(monsterPath);
        NPCInfoTableManager.LoadTable(npcPath);
    }

}
