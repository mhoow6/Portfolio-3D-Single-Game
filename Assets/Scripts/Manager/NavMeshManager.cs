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

    private void Awake()
    {
        instance = this;

        navMeshNames.Add("VillageNavMesh");
        navMeshNames.Add("ForestNavMesh");
        
        // Store NavMeshDatas
        foreach (string name in navMeshNames)
        {
            navMesh = Resources.Load<NavMeshData>("NavMeshData/" + name);
            navMeshDatas.Add(name, navMesh);
        }

    }

    public void CreateNavMesh(SceneType currentScene)
    {
        switch (currentScene)
        {
            case SceneType.Village:
                NavMesh.AddNavMeshData(navMeshDatas["VillageNavMesh"]);
                break;

            case SceneType.Forest:
                NavMesh.AddNavMeshData(navMeshDatas["ForestNavMesh"]);
                break;

            default:
                return;
        }
    }
}
