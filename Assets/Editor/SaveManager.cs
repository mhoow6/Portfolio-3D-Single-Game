using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class SaveManager
{
    const string MSG_FILTERED = "FILTERED";

    [MenuItem("Menu/Save/Scene Data")]
    public static void SaveScene()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Scene", path, "scene", "csv");
        SaveScene(path);

        Debug.Log("Scene Save Completed.");
    }

    [MenuItem("Menu/Save/Monster Position Data")]
    public static void SaveMonsterPosition()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Monster", path, "monster", "csv");
        SaveMonsterPosition(path);

        Debug.Log("Monster Position Save Completed.");
    }

    private static void SaveMonsterPosition(string filePath)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("index,id,xpos,ypos,zpos,xrot,yrot,zrot,xscale,yscale,zscale");

            for(int i = 0; i < monsters.Length; i++)
            {
                sw.WriteLine(
                    i.ToString() + "," +
                    MonsterNameToID(monsters[i].name) + "," +
                    monsters[i].transform.position.x + "," +
                    monsters[i].transform.position.y + "," +
                    monsters[i].transform.position.z + "," +
                    monsters[i].transform.rotation.eulerAngles.x + "," +
                    monsters[i].transform.rotation.eulerAngles.y + "," +
                    monsters[i].transform.rotation.eulerAngles.z + "," +
                    monsters[i].transform.localScale.x + "," +
                    monsters[i].transform.localScale.y + "," +
                    monsters[i].transform.localScale.z
                    );
            }
        }
    }

    private static void SaveScene(string filePath)
    {
        GameObject terrain = GameObject.FindGameObjectWithTag("Terrain");
        Transform[] terrain_children = terrain.GetComponentsInChildren<Transform>();

        GameObject rocks = GameObject.FindGameObjectWithTag("Rocks");
        Transform[] rocks_children = rocks.GetComponentsInChildren<Transform>();

        GameObject props = GameObject.FindGameObjectWithTag("Props");
        Transform[] props_children = props.GetComponentsInChildren<Transform>();

        GameObject vegetation = GameObject.FindGameObjectWithTag("Vegetation");
        Transform[] vegetation_children = vegetation.GetComponentsInChildren<Transform>();

        GameObject particle = GameObject.FindGameObjectWithTag("Particle");
        Transform[] particle_children = particle.GetComponentsInChildren<Transform>();

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(DataFromChildren(terrain_children));
            sw.Write(DataFromChildren(rocks_children));
            sw.Write(DataFromChildren(props_children));
            sw.Write(DataFromChildren(vegetation_children));
            sw.Write(DataFromChildren(particle_children));

            sw.Close();
        }
    }

    private static string FilteredName(string objName)
    {
        string[] splitName = objName.Split('_');
        string filteredName = string.Empty;

        for(int i = 0; i < splitName.Length; i++)
        {
            if (splitName[i] == "Lid" || splitName[i] == "LOD")
            {
                filteredName = MSG_FILTERED;
                break;
            }
                
            filteredName += splitName[i];

            if (i != splitName.Length - 1)
                filteredName += "_";
        }

        return filteredName;
    }

    private static string DataFromChildren(Transform[] children)
    {
        string result = string.Empty;

        for(int i = 0; i < children.Length; i++)
        {
            if (FilteredName(children[i].name.Split(' ')[0]) != MSG_FILTERED)
            {
                result +=
                    children[i].name.Split(' ')[0] + "," +
                    children[i].transform.position.x + "," +
                    children[i].transform.position.y + "," +
                    children[i].transform.position.z + "," +
                    children[i].transform.rotation.eulerAngles.x + "," +
                    children[i].transform.rotation.eulerAngles.y + "," +
                    children[i].transform.rotation.eulerAngles.z + "," +
                    children[i].transform.localScale.x + "," +
                    children[i].transform.localScale.y + "," +
                    children[i].transform.localScale.z;
                    result += "\n";
            }
        }

        return result;
    }

    private static ushort MonsterNameToID(string mobName)
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobName == mobinfo.monster_name)
                return mobinfo.id;
        }

        throw new System.NotSupportedException("몬스터중에" + mobName + " 은 없습니다.");
    }
}
