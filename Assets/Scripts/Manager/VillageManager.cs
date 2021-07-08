using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VillageManager : MapManager
{
    public GameObject village;

    private string villagePath;
    private string villageNPCPath;

    private void Awake()
    {
        villagePath = Application.persistentDataPath + "/Tables/Village.csv";
        villageNPCPath = Application.persistentDataPath + "/Tables/VillageNPCPosition.csv";

        SceneInfoManager.instance.currentScene = SceneType.Village;
    }

    private void Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        HUDManager.instance.loading.StartCoroutine(HUDManager.instance.loading.Loading(HUDManager.instance.loading._LOADING_SPEED));
        CreateScene(villagePath, village);
        CreatePlayer(SceneInfoManager.instance.spawnPos);
        CreateNPC(villageNPCPath);
        village.gameObject.SetActive(true);

        SceneInfoManager.instance.isSceneLoadCompleted = true;

        StartCoroutine(GoToTheNextScene(SceneType.Forest, SpawnPosID.VILLAGE_TO_FOREST));
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
