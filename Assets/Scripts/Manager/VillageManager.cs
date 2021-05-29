using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Text;


public class VillageManager : MapManager
{
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

        CreateScene(villagePath);
        CreatePlayer(villagePlayerPath);
        CreateNPC(villageNPCPath);

        StartCoroutine(GoToTheNextScene(SceneType.Forest));
    }

    private IEnumerator GoToTheNextScene(SceneType loadScene)
    {
        SceneInfoManager.instance.beforeScene = SceneInfoManager.instance.currentScene;
        WaitForSeconds wt = new WaitForSeconds(SceneInfoManager.instance.SCENE_ROAD_DURATION);

        while (true)
        {
            yield return wt;

            if (GameManager.instance.controller.player.transform.position.x > SceneInfoManager.instance.VILLAGE_TO_FOREST_MIN_X &&
                GameManager.instance.controller.player.transform.position.x < SceneInfoManager.instance.VILLAGE_TO_FOREST_MAX_X &&
                GameManager.instance.controller.player.transform.position.z > SceneInfoManager.instance.VILLAGE_TO_FOREST_MIN_Z &&
                GameManager.instance.controller.player.transform.position.z < SceneInfoManager.instance.VILLAGE_TO_FOREST_MAX_Z)
                    SceneManager.LoadScene((int)loadScene);
        }
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
