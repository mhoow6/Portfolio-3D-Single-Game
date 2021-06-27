using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectInfo
{
    public ushort id;
    public string prefab_name;
}

public static class EffectInfoTableManager
{
    public static List<EffectInfo> effectList = new List<EffectInfo>();
    
    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            EffectInfo effectInfo;

            effectInfo.id = ushort.Parse(datas[0]);
            effectInfo.prefab_name = datas[1];

            effectList.Add(effectInfo);
        }
    }

    public static ushort GetEffectIDFromPrefabName(string prefabName)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].prefab_name == prefabName)
                return effectList[i].id;
        }

        throw new System.NotSupportedException(prefabName + " 에 해당하는 이펙트가 없습니다.");
    }
}
