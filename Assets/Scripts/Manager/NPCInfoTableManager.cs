using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct NpcInfo
{
    public ushort id;
    public string name;
    public byte npc_type;
}

public static class NPCInfoTableManager
{
    public static List<NpcInfo> npcInfoList = new List<NpcInfo>();

    public static void LoadTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            sr.ReadLine();

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                NpcInfo npcInfo;

                npcInfo.id = ushort.Parse(datas[0]);
                npcInfo.name = datas[1];
                npcInfo.npc_type = byte.Parse(datas[2]);

                npcInfoList.Add(npcInfo);
            }

            sr.Close();
        }
    }
}

