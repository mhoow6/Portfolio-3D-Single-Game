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
    }

    [MenuItem("Menu/Save/Monster Data")]
    public static void SaveMonster()
    {
        string path = string.Empty;
        path = EditorUtility.SaveFilePanel("Save Monster", path, "monster", "csv");
        SaveMonster(path);
    }

    private static void SaveMonster(string filePath)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            foreach (GameObject monster in monsters)
            {
                sw.WriteLine(
                    monster.name + "," +
                    monster.transform.position.x + "," +
                    monster.transform.position.z
                    );
            }
        }

        Debug.Log("Monster Save Completed.");
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

        Debug.Log("Scene Save Completed.");
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
}
