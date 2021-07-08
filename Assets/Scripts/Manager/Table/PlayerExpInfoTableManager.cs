using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerExpInfo
{
    public byte level;
    public float max_exp;
}


public static class PlayerExpInfoTableManager
{
    const int MAX_LEVEL = 10;

    public static PlayerExpInfo[] playerExpInfo = new PlayerExpInfo[MAX_LEVEL];

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTableFileStream(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerExpInfo[i - 1].level = byte.Parse(datas[0]);
            playerExpInfo[i - 1].max_exp = float.Parse(datas[1]);
        }
    }

    public static ref PlayerExpInfo GetPlayerExpInfoFromLevel(byte playerLevel)
    {
        for (int i = 0; i < playerExpInfo.Length; i++)
        {
            if (playerExpInfo[i].level == playerLevel)
                return ref playerExpInfo[i];
        }

        throw new System.NotSupportedException(playerLevel + " 에 해당하는 정보들이 없습니다.");
    }

}

