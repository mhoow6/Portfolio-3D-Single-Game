using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerSpawnInfo
{
    public ushort id;
    public float xpos;
    public float ypos;
    public float zpos;
    public float xrot;
    public float yrot;
    public float zrot;
    public float xscale;
    public float yscale;
    public float zscale;
}

public enum SpawnPosID
{
    VILLAGE_START = 100,
    FOREST_TO_VILLAGE = 101,
    VILLAGE_TO_FOREST = 200
}

public static class PlayerSpawnInfoTableManager
{
    public static List<PlayerSpawnInfo> playerSpawnInfoList = new List<PlayerSpawnInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        PlayerSpawnInfo playerSpawnInfo; // temp

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerSpawnInfo.id = ushort.Parse(datas[0]);
            playerSpawnInfo.xpos = float.Parse(datas[1]);
            playerSpawnInfo.ypos = float.Parse(datas[2]);
            playerSpawnInfo.zpos = float.Parse(datas[3]);
            playerSpawnInfo.xrot = float.Parse(datas[4]);
            playerSpawnInfo.yrot = float.Parse(datas[5]);
            playerSpawnInfo.zrot = float.Parse(datas[6]);
            playerSpawnInfo.xscale = float.Parse(datas[7]);
            playerSpawnInfo.yscale = float.Parse(datas[8]);
            playerSpawnInfo.zscale = float.Parse(datas[9]);

            playerSpawnInfoList.Add(playerSpawnInfo);
        }
    }
}
