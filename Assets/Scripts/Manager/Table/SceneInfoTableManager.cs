using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SceneInfo
{
    public ushort scene_index;
    public string scene_name;
}

public static class SceneInfoTableManager
{
    public static List<SceneInfo> sceneInfoList = new List<SceneInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        SceneInfo sceneInfo; // temp

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');
            
            sceneInfo.scene_index = ushort.Parse(datas[0]);
            sceneInfo.scene_name = datas[1];

            sceneInfoList.Add(sceneInfo);
        }
    }
}
