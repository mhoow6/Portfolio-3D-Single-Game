using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class FieldManager : MapManager
{
    private string forestPath;
    private string forestMonsterPath;
    private string forestPlayerPath;
    

    private void Awake()
    {
        forestPath = "Tables/Forest";
        forestMonsterPath = "Tables/ForestMonsterPosition";
        forestPlayerPath = "Tables/ForestPlayerPosition";

        SceneInfoManager.instance.currentScene = SceneType.Forest;
    }

    private void Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        CreateScene(forestPath);
        CreatePlayer(forestPlayerPath);
        CreateMonster(forestMonsterPath);

        StartCoroutine(GoToTheNextScene(SceneType.Village));
    }

    private IEnumerator GoToTheNextScene(SceneType loadScene)
    {
        SceneInfoManager.instance.beforeScene = SceneInfoManager.instance.currentScene;
        WaitForSeconds wt = new WaitForSeconds(SceneInfoManager.instance.SCENE_ROAD_DURATION);

        while (true)
        {
            yield return wt;

            if (GameManager.instance.controller.player.transform.position.x < SceneInfoManager.instance.FOREST_TO_VILLAGE_MIN_X &&
                GameManager.instance.controller.player.transform.position.z > SceneInfoManager.instance.FOREST_TO_VILLAGE_MIN_Z &&
                GameManager.instance.controller.player.transform.position.z < SceneInfoManager.instance.FOREST_TO_VILLAGE_MAX_Z)
                SceneManager.LoadScene((int)loadScene);
        }
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
