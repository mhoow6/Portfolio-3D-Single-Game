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

    private IEnumerator Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        CreateScene(villagePath, village);
        CreatePlayer(SceneInfoManager.instance.spawnPos);
        CreateNPC(villageNPCPath);
        SceneInfoManager.instance.isSceneLoadCompleted = true;
        yield return HUDManager.instance.loading.StartCoroutine(HUDManager.instance.loading.Loading(HUDManager.instance.loading._LOADING_SPEED));

        village.gameObject.SetActive(true);
        GameManager.instance.BGM = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_VILLAGE, AudioCondition.SCENE_AWAKE));
        GameManager.instance.BGM.loop = true;
        AudioManager.instance.PlayAudioFadeIn(GameManager.instance.BGM, AudioManager.instance._VILLAGE_SOUND);
        
        StartCoroutine(GoToTheNextScene(SceneType.Forest, SpawnPosID.VILLAGE_TO_FOREST));
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
