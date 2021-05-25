using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager instance;
    static Dictionary<string, NavMeshData> navMeshDatas = new Dictionary<string, NavMeshData>();
    static List<string> navMeshNames = new List<string>();

    NavMeshData navMesh;
    string navMeshDirectory;
    string navMeshData_Test = "TestNavMesh";
    string navMeshData_Village = "VillageNavMesh";
    string navMeshData_Forest = "ForestNavMesh";

    private void Awake()
    {
        instance = this;

        navMeshDirectory = Application.dataPath + "/" + "Resources" + "/" + "NavMeshData" + "/";
        
        // Get NavMeshData From Directory And Stored to list.
        GetAllNavMeshName(navMeshDirectory);

        // Store NavMeshDatas
        foreach (string name in navMeshNames)
        {
            navMesh = Resources.Load<NavMeshData>("NavMeshData/" + name);
            navMeshDatas.Add(name, navMesh);
        }

    }

    private void GetAllNavMeshName(string directory)
    {
        if (!Directory.Exists(directory))
            return;

        string filter = "*.asset";
        string[] files = Directory.GetFiles(directory, filter);

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(directory, "");
            files[i] = files[i].Replace(".asset", "");
        }
            
        foreach (string file in files)
            navMeshNames.Add(file);
    }

    // navMeshNames로부터 가져오게 끔 만들자..
    public void CreateNavMesh(SceneType currentScene)
    {
        switch (currentScene)
        {
            case SceneType.Village:
                NavMesh.AddNavMeshData(navMeshDatas["VillageNavMesh"]);
                Debug.Log("Village NavMesh");
                break;

            case SceneType.Forest:
                NavMesh.AddNavMeshData(navMeshDatas["ForestNavMesh"]);
                Debug.Log("Forest NavMesh");
                break;

            default:
                return;
        }
    }
}
