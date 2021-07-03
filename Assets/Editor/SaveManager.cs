using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;

public static class SaveManager
{
    [MenuItem("Menu/Save/Scene Data")]
    public static void SaveScene()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Scene", path, "scene", "csv");
        SaveScene(path);

        Debug.Log("Scene Save Completed.");
    }

    [MenuItem("Menu/Save/Monster Position Data")]
    public static void SaveMonsterPosition()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Monster", path, "monster", "csv");
        SaveMonsterPosition(path);

        Debug.Log("Monster Position Save Completed.");
    }

    [MenuItem("Menu/Save/Player Position Data")]
    public static void SavePlayerPosition()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Monster", path, "monster", "csv");
        SavePlayerPosition(path);

        Debug.Log("Player Position Save Completed.");
    }

    [MenuItem("Menu/Save/NPC Position Data")]
    public static void SaveNPCPosition()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save NPC", path, "npc", "csv");
        SaveNPCPosition(path);

        Debug.Log("NPC Position Save Completed.");
    }

    [MenuItem("Menu/Save/Generate Collider")]
    public static void GenerateCollider()
    {
        string path = Application.dataPath + "/Resources/Result/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            throw new DirectoryNotFoundException("디렉토리가 없어서 콜라이더 생성에 실패했습니다. 디렉토리를 만들었으니 다시 시도해주세요.");
        }

        ColliderGenerator _instanceRoot = GameObject.FindObjectOfType<ColliderGenerator>();
        GameObject instanceRoot; // Temp
        string instancePath;

        if (_instanceRoot.childrenMeshes.Count == 0)
            _instanceRoot.ColliderGenerate();

        for (int i = 0; i < _instanceRoot.childrenMeshes.Count; i++)
        {
            instanceRoot = _instanceRoot.childrenMeshes[i].gameObject;
            instancePath = path + instanceRoot.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(instanceRoot, instancePath, out bool success);
            Debug.Log($"{instanceRoot.name} save {success == true}");
        }

        for (int i = 0; i < _instanceRoot.childrenSkinnedMeshes.Count; i++)
        {
            instanceRoot = _instanceRoot.childrenSkinnedMeshes[i].transform.parent.gameObject;
            instancePath = path + instanceRoot.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(instanceRoot, instancePath, out bool success);
            Debug.Log($"{instanceRoot.name} save {success == true}");
        }

        Debug.Log("박스 콜라이더 생성이 완료되었습니다. 콜라이더의 센터는 오브젝트마다 다르니 필요시 조정하세요.");
    }

    private static void SaveMonsterPosition(string filePath)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        string data = string.Empty;

        data = "index,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale";
        data += "\n";

        for (int i = 0; i < monsters.Length; i++)
        {
            data +=
            i.ToString() + "," +
            GetMonsterIDFromName(EraseBracketInName(monsters[i].name)) + "," +
            monsters[i].transform.position.x + "," +
            monsters[i].transform.position.y + "," +
            monsters[i].transform.position.z + "," +
            monsters[i].transform.rotation.eulerAngles.x + "," +
            monsters[i].transform.rotation.eulerAngles.y + "," +
            monsters[i].transform.rotation.eulerAngles.z + "," +
            monsters[i].transform.localScale.x + "," +
            monsters[i].transform.localScale.y + "," +
            monsters[i].transform.localScale.z;          
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(filePath, buffer);
    }

    private static void SavePlayerPosition(string filePath)
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        string data = string.Empty;

        data = "index,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale";
        data += "\n";

        for (int i = 0; i < player.Length; i++)
        {
            data += i + "," +
                player[i].transform.position.x + "," +
                player[i].transform.position.y + "," +
                player[i].transform.position.z + "," +
                player[i].transform.rotation.eulerAngles.x + "," +
                player[i].transform.rotation.eulerAngles.y + "," +
                player[i].transform.rotation.eulerAngles.z + "," +
                player[i].transform.localScale.x + "," +
                player[i].transform.localScale.y + "," +
                player[i].transform.localScale.z;
            data += "\n";
        }
            
        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(filePath, buffer);
    }

    private static void SaveNPCPosition(string filePath)
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        string data = string.Empty;

        data = "index,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale";
        data += "\n";

        for (int i = 0; i < npcs.Length; i++)
        {
            data +=
            i.ToString() + "," +
            GetNPCIDFromName(npcs[i].name) + "," +
            npcs[i].transform.position.x + "," +
            npcs[i].transform.position.y + "," +
            npcs[i].transform.position.z + "," +
            npcs[i].transform.rotation.eulerAngles.x + "," +
            npcs[i].transform.rotation.eulerAngles.y + "," +
            npcs[i].transform.rotation.eulerAngles.z + "," +
            npcs[i].transform.localScale.x + "," +
            npcs[i].transform.localScale.y + "," +
            npcs[i].transform.localScale.z;
            data += "\n";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(data);
        File.WriteAllBytes(filePath, buffer);
    }

    private static void SaveScene(string filePath)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("name,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale");

            GameObject parent = GameObject.FindGameObjectWithTag("Terrain");
            sw.WriteLine(parent.name);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                sw.WriteLine(
                    EraseBracketInName(parent.transform.GetChild(i).name) + "," +
                    parent.transform.GetChild(i).position.x + "," +
                    parent.transform.GetChild(i).position.y + "," +
                    parent.transform.GetChild(i).position.z + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.x + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.y + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.z + "," +
                    parent.transform.GetChild(i).localScale.x + "," +
                    parent.transform.GetChild(i).localScale.y + "," +
                    parent.transform.GetChild(i).localScale.z
                    );
            }

            // 반복
            parent = GameObject.FindGameObjectWithTag("Rocks");
            sw.WriteLine(parent.name);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                sw.WriteLine(
                    EraseBracketInName(parent.transform.GetChild(i).name) + "," +
                    parent.transform.GetChild(i).position.x + "," +
                    parent.transform.GetChild(i).position.y + "," +
                    parent.transform.GetChild(i).position.z + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.x + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.y + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.z + "," +
                    parent.transform.GetChild(i).localScale.x + "," +
                    parent.transform.GetChild(i).localScale.y + "," +
                    parent.transform.GetChild(i).localScale.z
                    );
            }
            // 반복

            parent = GameObject.FindGameObjectWithTag("Props");
            sw.WriteLine(parent.name);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                sw.WriteLine(
                    EraseBracketInName(parent.transform.GetChild(i).name) + "," +
                    parent.transform.GetChild(i).position.x + "," +
                    parent.transform.GetChild(i).position.y + "," +
                    parent.transform.GetChild(i).position.z + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.x + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.y + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.z + "," +
                    parent.transform.GetChild(i).localScale.x + "," +
                    parent.transform.GetChild(i).localScale.y + "," +
                    parent.transform.GetChild(i).localScale.z
                    );
            }

            parent = GameObject.FindGameObjectWithTag("Vegetation");
            sw.WriteLine(parent.name);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                sw.WriteLine(
                    EraseBracketInName(parent.transform.GetChild(i).name) + "," +
                    parent.transform.GetChild(i).position.x + "," +
                    parent.transform.GetChild(i).position.y + "," +
                    parent.transform.GetChild(i).position.z + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.x + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.y + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.z + "," +
                    parent.transform.GetChild(i).localScale.x + "," +
                    parent.transform.GetChild(i).localScale.y + "," +
                    parent.transform.GetChild(i).localScale.z
                    );
            }

            parent = GameObject.FindGameObjectWithTag("Particle");
            sw.WriteLine(parent.name);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                sw.WriteLine(
                    EraseBracketInName(parent.transform.GetChild(i).name) + "," +
                    parent.transform.GetChild(i).position.x + "," +
                    parent.transform.GetChild(i).position.y + "," +
                    parent.transform.GetChild(i).position.z + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.x + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.y + "," +
                    parent.transform.GetChild(i).rotation.eulerAngles.z + "," +
                    parent.transform.GetChild(i).localScale.x + "," +
                    parent.transform.GetChild(i).localScale.y + "," +
                    parent.transform.GetChild(i).localScale.z
                    );
            }

            sw.Close();
        }

        Debug.Log("Scene Save Completed.");
    }

    private static ushort GetMonsterIDFromName(string mobName)
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobName == mobinfo.monster_name)
                return mobinfo.id;
        }

        throw new System.NotSupportedException("몬스터 중에 " + mobName + " 은 없습니다.");
    }

    private static ushort GetNPCIDFromName(string npcName)
    {
        string npcInfoPath = Application.dataPath + "/Resources/Tables/NPCInfo.csv";

        NPCInfoTableManager.LoadTable(npcInfoPath);

        foreach (NpcInfo npcinfo in NPCInfoTableManager.npcInfoList)
        {
            if (npcName == npcinfo.npc_name)
                return npcinfo.id;
        }

        throw new System.NotSupportedException("몬스터 중에 " + npcName + " 은 없습니다.");
    }

    private static string EraseBracketInName(string mobName)
    {
        string mobNameWithNoSpace = mobName.Replace(" ", "");
        int index = mobNameWithNoSpace.IndexOf('(');

        if (index == -1)
            return mobName;

        return mobNameWithNoSpace.Remove(index);
    }
}
