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
            throw new DirectoryNotFoundException("���丮�� ��� �ݶ��̴� ������ �����߽��ϴ�. ���丮�� ��������� �ٽ� �õ����ּ���.");
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

        Debug.Log("�ڽ� �ݶ��̴� ������ �Ϸ�Ǿ����ϴ�. �ݶ��̴��� ���ʹ� ������Ʈ���� �ٸ��� �ʿ�� �����ϼ���.");
    }

    [MenuItem("Menu/Save/Mesh Combined Objects")]
    public static void MeshCombinedObjects()
    {
        // MeshCombine ������Ʈ�� ���� ���ӿ�����Ʈ���� ã�� �迭�� �����Ѵ�. 
        MeshCombine[] _instanceRoots = GameObject.FindObjectsOfType<MeshCombine>();

        GameObject instanceRoot; // Temp
        Mesh instanceRootMesh; // Temp
        string instancePath; // Temp

        for (int i = 0; i < _instanceRoots.Length; i++)
        {
            string resultPath = Application.dataPath + "/Resources/Result/";
            string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", resultPath, _instanceRoots[i].name + "_Mesh", "asset");

            if (string.IsNullOrEmpty(path)) return;

            path = FileUtil.GetProjectRelativePath(path); // For AssetDatabase.CreateAsset

            // ������ �Ž� �Ĺ����� �����Ѵ�.
            _instanceRoots[i].MeshCombineObjects();

            // �Ž� �Ĺ����� �� ���ӿ�����Ʈ
            instanceRoot = _instanceRoots[i].gameObject;

            // �Ž� �Ĺ����� �� ���ӿ�����Ʈ�� ���������� ������ ���� �̸�
            instancePath = resultPath + _instanceRoots[i].name + ".prefab";

            // �Ž� �Ĺ����� �� ���ӿ�����Ʈ�� �Ž��� �ν��Ͻ�
            instanceRootMesh = Object.Instantiate(_instanceRoots[i].selfMeshFilter.sharedMesh);

            // �ν��Ͻ̵� �Ž��� Asset���ν� �����Ѵ�. ���� �Ž��� ������ ���������� ������ �� �� �Ž��� ���� ä�� �����.
            AssetDatabase.CreateAsset(instanceRootMesh, path);
            AssetDatabase.SaveAssets();

            // �ν��Ͻ� �Ž��� �ٽ� ������ sharedMesh�� �ο��Ѵ�. (�����տ� �Ž� �ڵ� �ο��뵵)
            _instanceRoots[i].selfMeshFilter.sharedMesh = instanceRootMesh;

            // �Ž� �Ĺ��ε� ���ӿ�����Ʈ�� ���������ν� �����Ѵ�.
            PrefabUtility.SaveAsPrefabAsset(instanceRoot, instancePath, out bool success);

            if (success == true)
                Debug.Log(instanceRoot.name + " �ȿ� �����ִ� ������Ʈ���� �Ž� �Ĺ��ο� �����Ͽ����ϴ�!");

            if (success == false)
                Debug.Log(instanceRoot.name + " �ȿ� �����ִ� ������Ʈ���� �Ž� �Ĺ��ο� �����Ͽ����ϴ�...");

            _instanceRoots[i].Clean();
        }     
    }

    [MenuItem("Menu/Save/Mesh Combined Object")]
    public static void MeshCombinedObject()
    {
        string resultPath = Application.dataPath + "/Resources/Result/";
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", resultPath, "Mesh", "asset");

        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path); // For AssetDatabase.CreateAsset

        // MeshCombine ������Ʈ�� ���� ���ӿ�����Ʈ���� ã�� �迭�� �����Ѵ�. 
        MeshCombine _instanceRoot = GameObject.FindObjectOfType<MeshCombine>();

        GameObject instanceRoot; // Temp
        Mesh instanceRootMesh; // Temp
        string instancePath; // Temp

        // ������ �Ž� �Ĺ����� �����Ѵ�.
        _instanceRoot.MeshCombineObjects();

        // �Ž� �Ĺ����� �� ���ӿ�����Ʈ
        instanceRoot = _instanceRoot.gameObject;

        // �Ž� �Ĺ����� �� ���ӿ�����Ʈ�� ���������� ������ ���� �̸�
        instancePath = resultPath + _instanceRoot.name + ".prefab";

        // �Ž� �Ĺ����� �� ���ӿ�����Ʈ�� �Ž��� �ν��Ͻ�
        instanceRootMesh = Object.Instantiate(_instanceRoot.selfMeshFilter.sharedMesh);

        // �ν��Ͻ̵� �Ž��� Asset���ν� �����Ѵ�.
        AssetDatabase.CreateAsset(instanceRootMesh, path);
        AssetDatabase.SaveAssets();

        // �ν��Ͻ� �Ž��� �ٽ� ������ sharedMesh�� �ο��Ѵ�. (�����տ� �Ž� �ڵ� �ο��뵵)
        _instanceRoot.selfMeshFilter.sharedMesh = instanceRootMesh;

        // �Ž� �Ĺ��ε� ���ӿ�����Ʈ�� ���������ν� �����Ѵ�.
        PrefabUtility.SaveAsPrefabAsset(instanceRoot, instancePath, out bool success);

        if (success == true)
            Debug.Log(instanceRoot.name + " �ȿ� �����ִ� ������Ʈ���� �Ž� �Ĺ��ο� �����Ͽ����ϴ�!");

        if (success == false)
            Debug.Log(instanceRoot.name + " �ȿ� �����ִ� ������Ʈ���� �Ž� �Ĺ��ο� �����Ͽ����ϴ�...");

        // ���� �÷��� ������Ʈ�� �ʱ���·� ����
        _instanceRoot.Clean();
    }

    private static void SaveMonsterPosition(string filePath)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        string data = string.Empty;

        data = "index,id,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale";
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
            data += "\n";
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

            // �ݺ�
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
            // �ݺ�

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

        MonsterInfoTableManager.LoadTableForSaveManager(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobName == mobinfo.prefab_name)
                return mobinfo.id;
        }

        throw new System.NotSupportedException("���� �߿� " + mobName + " �� �����ϴ�.");
    }

    private static ushort GetNPCIDFromName(string npcName)
    {
        string npcInfoPath = Application.dataPath + "/Resources/Tables/NPCInfo.csv";

        NPCInfoTableManager.LoadTableForSaveManager(npcInfoPath);

        foreach (NpcInfo npcinfo in NPCInfoTableManager.npcInfoList)
        {
            if (npcName == npcinfo.prefab_name)
                return npcinfo.id;
        }

        throw new System.NotSupportedException("���� �߿� " + npcName + " �� �����ϴ�.");
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
