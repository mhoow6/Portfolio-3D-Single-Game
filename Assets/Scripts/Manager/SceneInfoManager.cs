using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

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
    public Vector2 ROOM_MAP_RADIUS = new Vector2(60f, 60f);
    public Vector2 ROOM_MINIMAP_CENTER = new Vector2(0, 0);
    public Vector3 VILLAGE_MINIMAP_CENTER = new Vector3(3.8f, 0f, 0.65f);
    public Vector2 VILLAGE_MAP_RADIUS = new Vector2(45f, 45f);
    public Vector3 FOREST_MINIMAP_CENTER = new Vector3(-5f, 0f, 0f);
    public Vector2 FOREST_MAP_RADIUS = new Vector2(170f, 50f);
    public bool isTableManagerAwakeOnce;
    public bool isTempDataExists;
    public bool isSceneLoadCompleted;
}
