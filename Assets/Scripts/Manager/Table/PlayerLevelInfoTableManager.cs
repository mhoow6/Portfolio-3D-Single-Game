using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerLevelInfo
{
    public byte level;
    public float hp;
    public float mp;
    public float sp;
}


public static class PlayerLevelInfoTableManager
{
    const int MAX_LEVEL = 10;

    public static PlayerLevelInfo[] playerLevelInfo = new PlayerLevelInfo[MAX_LEVEL];

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerLevelInfo[i - 1].level = byte.Parse(datas[0]);
            playerLevelInfo[i - 1].hp = float.Parse(datas[1]);
            playerLevelInfo[i - 1].mp = float.Parse(datas[2]);
            playerLevelInfo[i - 1].sp = float.Parse(datas[3]);
        }
    }

    public static ref PlayerLevelInfo GetPlayerLevelInfoFromLevel(byte playerLevel)
    {
        for (int i = 0; i < playerLevelInfo.Length; i++)
        {
            if (playerLevelInfo[i].level == playerLevel)
                return ref playerLevelInfo[i];
        }

        throw new System.NotSupportedException(playerLevel + " 에 해당하는 정보들이 없습니다.");
    }
   
}

