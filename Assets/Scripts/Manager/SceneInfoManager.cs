using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Village,
    Forest
}


public class SceneInfoManager : SingleTon<SceneInfoManager>
{
    public SceneType beforeScene;
    public SceneType currentScene;
    public float SCENE_ROAD_DURATION = 0.5f;
    public float FOREST_TO_VILLAGE_MIN_X = -76.231f;
    public float FOREST_TO_VILLAGE_MIN_Z = -5.25178f;
    public float FOREST_TO_VILLAGE_MAX_Z = -2.95f;
    public float VILLAGE_TO_FOREST_MIN_X = 16f;
    public float VILLAGE_TO_FOREST_MAX_X = 17f;
    public float VILLAGE_TO_FOREST_MIN_Z = -0.76f;
    public float VILLAGE_TO_FOREST_MAX_Z = 2.067f;
}
