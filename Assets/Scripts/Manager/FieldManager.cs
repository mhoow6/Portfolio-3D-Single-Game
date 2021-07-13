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
        forestPath = Application.persistentDataPath + "/Tables/Forest.csv";
        forestMonsterPath = Application.persistentDataPath + "/Tables/ForestMonsterPosition.csv";

        SceneInfoManager.instance.currentScene = SceneType.Forest;
    }

    private IEnumerator Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.instance.currentScene);

        HUDManager.instance.loading.StartCoroutine(HUDManager.instance.loading.Loading(HUDManager.instance.loading._LOADING_SPEED));
        CreateScene(forestPath, field);
        CreatePlayer(SceneInfoManager.instance.spawnPos);
        CreateMonster(forestMonsterPath);
        SceneInfoManager.instance.isSceneLoadCompleted = true;
        yield return HUDManager.instance.loading.StartCoroutine(HUDManager.instance.loading.Loading(HUDManager.instance.loading._LOADING_SPEED));

        field.gameObject.SetActive(true);
        GameManager.instance.BGM = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_FOREST, AudioCondition.SCENE_AWAKE));
        GameManager.instance.BGM.loop = true;
        AudioManager.instance.PlayAudioFadeIn(GameManager.instance.BGM, AudioManager.instance._FOREST_SOUND);

        StartCoroutine(GoToTheNextScene(SceneType.Village, SpawnPosID.FOREST_TO_VILLAGE));
    }

    private void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }
}
