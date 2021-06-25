using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class VillageManager : MapManager
{
    public GameObject village;

    private string villagePath;
    private string villageNPCPath;
    private string villagePlayerPath;

    private void Awake()
    {
        villagePath = "Tables/Village";
        villageNPCPath = "Tables/VillageNPCPosition";
        villagePlayerPath = "Tables/VillagePlayerPosition";

        SceneInfoManager.instance.currentScene = SceneType.Village;
    }

    private void Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        CreateScene(villagePath, village);
        CreatePlayer(villagePlayerPath);
        CreateNPC(villageNPCPath);

        HUDManager.instance.loading.gameObject.SetActive(false);
        village.gameObject.SetActive(true);

        StartCoroutine(GoToTheNextScene(SceneType.Forest));
    }

    private IEnumerator GoToTheNextScene(SceneType loadScene)
    {
        SceneInfoManager.instance.isSceneLoadCompleted = false;
        SceneInfoManager.instance.beforeScene = SceneInfoManager.instance.currentScene;
        WaitForSeconds wt = new WaitForSeconds(SceneInfoManager.instance.SCENE_ROAD_DURATION);

        while (true)
        {
            yield return wt;

            if (GameManager.instance.controller.player.transform.position.x > SceneInfoManager.instance.VILLAGE_TO_FOREST_MIN_X &&
                GameManager.instance.controller.player.transform.position.x < SceneInfoManager.instance.VILLAGE_TO_FOREST_MAX_X &&
                GameManager.instance.controller.player.transform.position.z > SceneInfoManager.instance.VILLAGE_TO_FOREST_MIN_Z &&
                GameManager.instance.controller.player.transform.position.z < SceneInfoManager.instance.VILLAGE_TO_FOREST_MAX_Z)
            {
                SceneManager.LoadScene((int)loadScene);
                HUDManager.instance.system.SaveGame();
            }
                    
        }
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
