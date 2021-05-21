using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Village,
    Forest
}

public static class SceneInfoManager
{
    public static SceneType beforeScene;
    public static SceneType currentScene;
    public static bool isTableManagerAwakeOnce;
}
