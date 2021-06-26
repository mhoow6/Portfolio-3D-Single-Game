using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class FieldManager : MapManager
{
    public GameObject field;

    private string forestPath;
    private string forestMonsterPath;

    private void Awake()
    {
        forestPath = "Tables/Forest";
        forestMonsterPath = "Tables/ForestMonsterPosition";

        SceneInfoManager.instance.currentScene = SceneType.Forest;
    }

    private void Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        CreateScene(forestPath, field);
        CreatePlayer(SceneInfoManager.instance.spawnPos);
        CreateMonster(forestMonsterPath);
        field.gameObject.SetActive(true);

        SceneInfoManager.instance.isSceneLoadCompleted = true;

        StartCoroutine(GoToTheNextScene(SceneType.Village, SpawnPosID.FOREST_TO_VILLAGE));
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
