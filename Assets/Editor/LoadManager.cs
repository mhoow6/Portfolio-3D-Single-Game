using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class LoadManager
{
    [MenuItem("Menu/Load/Scene Data")]
    public static void LoadScene()
    {
        string path = string.Empty;
        path = EditorUtility.OpenFilePanel("Load Scene", path, "csv");
        LoadScene(path);
    }

    [MenuItem("Menu/Load/Monster Data")]
    public static void LoadMonster()
    {
        string path = string.Empty;
        path = EditorUtility.OpenFilePanel("Load Monster", path, "csv");
        LoadMonster(path);
    }

    private static void LoadMonster(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Monster");

            while((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                string objName = datas[0];
                float xPos = float.Parse(datas[1]);
                float zPos = float.Parse(datas[2]);

                GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + objName);
                GameObject obj = GameObject.Instantiate(_obj);

                Monster monster = Monster.AddMonsterComponent(obj, objName);
                monster.name = objName;

                monster.transform.position = Utility.RayToDown(new Vector3(xPos, 0, zPos));

                monster.transform.SetParent(parent.transform);
            }
        }
    }

    private static void LoadScene(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = string.Empty;
            string path = string.Empty;
            GameObject parent = null;

            while((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                string objName = datas[0];

                // 1. ���� ������ ��������
                if (objName == "")
                    continue;

                // 2. ���ҽ� ���� ����
                if (objName == "Particle" || objName == "Props" || objName == "Vegetation" || objName == "Rocks" || objName == "Terrain" || objName == "Plane")
                {
                    path = objName;
                    parent = new GameObject(objName);
                    continue;
                }
                
                // 3. ���̺� ������ ����
                float xPos = float.Parse(datas[1]);
                float yPos = float.Parse(datas[2]);
                float zPos = float.Parse(datas[3]);
                Vector3 objPos = new Vector3(xPos, yPos, zPos);
                float xRot = float.Parse(datas[4]);
                float yRot = float.Parse(datas[5]);
                float zRot = float.Parse(datas[6]);
                Vector3 objAngle = new Vector3(xRot, yRot, zRot);
                float xScale = float.Parse(datas[7]);
                float yScale = float.Parse(datas[8]);
                float zScale = float.Parse(datas[9]);
                Vector3 objScale = new Vector3(xScale, yScale, zScale);

                // 4. �ε� & �ν��Ͻ�
                GameObject _obj = Resources.Load<GameObject>(path + "/" + objName);
                GameObject obj = GameObject.Instantiate(_obj);

                // 5. ���� ���ӿ�����Ʈ�� ���̺� ������ ����
                obj.name = objName;
                obj.transform.position = objPos;
                obj.transform.rotation = Quaternion.Euler(objAngle);
                obj.transform.localScale = objScale;

                // 6. �θ� ������Ʈ�� ����
                obj.transform.SetParent(parent.transform);
            }

            Debug.Log("Scene Load Completed.");
        }
    }
}
