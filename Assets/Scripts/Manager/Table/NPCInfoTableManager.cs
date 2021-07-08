using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct NpcInfo
{
    public ushort id;
    public string prefab_name;
    public string npc_name;
    public byte npc_type;
}

public static class NPCInfoTableManager
{
    public static List<NpcInfo> npcInfoList = new List<NpcInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTableFileStream(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            NpcInfo npcInfo;

            npcInfo.id = ushort.Parse(datas[0]);
            npcInfo.prefab_name = datas[1];
            npcInfo.npc_name = datas[2];
            npcInfo.npc_type = byte.Parse(datas[3]);

            npcInfoList.Add(npcInfo);
        }

    }

    public static void LoadTableForSaveManager(string filePath)
    {
        List<string> lines = GetLinesFromTableForSaveManager(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            NpcInfo npcInfo;

            npcInfo.id = ushort.Parse(datas[0]);
            npcInfo.prefab_name = datas[1];
            npcInfo.npc_name = datas[2];
            npcInfo.npc_type = byte.Parse(datas[3]);

            npcInfoList.Add(npcInfo);
        }

    }

    public static string GetNPCNameFromID(ushort npcID)
    {
        foreach (NpcInfo npcInfo in npcInfoList)
        {
            if (npcID == npcInfo.id)
                return npcInfo.npc_name;
        }

        throw new System.NotSupportedException(npcID + " 에 해당하는 NPC는 없습니다.");
    }

    public static string GetNPCPrefabFromID(ushort npcID)
    {
        foreach (NpcInfo npcInfo in npcInfoList)
        {
            if (npcID == npcInfo.id)
                return npcInfo.prefab_name;
        }

        throw new System.NotSupportedException(npcID + " 에 해당하는 NPC는 없습니다.");
    }

    private static List<string> GetLinesFromTableForSaveManager(string filePath)
    {
        string line = string.Empty;
        List<string> lines = new List<string>();

        Debug.Log(filePath);

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

