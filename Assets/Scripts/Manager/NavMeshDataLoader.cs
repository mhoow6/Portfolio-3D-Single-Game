using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDataLoader : MonoBehaviour
{
    NavMeshData m_NavMesh;
    string navMeshData_Village = "VillageNavMesh";
    string navMeshData_Forest = "ForestNavMesh";

    private void Awake()
    {
        switch (SceneInfoManager.currentScene)
        {
            case SceneType.Village:
                m_NavMesh = Resources.Load<NavMeshData>("NavMeshData/" + navMeshData_Village);
                break;

            case SceneType.Forest:
                m_NavMesh = Resources.Load<NavMeshData>("NavMeshData/" + navMeshData_Forest);
                break;

            default:
                m_NavMesh = null;
                break;
        }

        NavMesh.AddNavMeshData(m_NavMesh);
    }
}
