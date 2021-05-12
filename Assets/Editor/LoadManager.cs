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

        Debug.Log("Scene Load Completed.");
    }

    [MenuItem("Menu/Load/Monster Position Data")]
    public static void LoadMonsterPosition()
    {
        string path = string.Empty;
        path = EditorUtility.OpenFilePanel("Load Monster", path, "csv");
        LoadMonsterPosition(path);

        Debug.Log("Monster Position Load Completed.");
    }

    private static void LoadMonsterPosition(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Monster");

            sr.ReadLine(); // ù ���ڵ� ��ŵ

            while((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                ushort index = ushort.Parse(datas[0]);
                ushort id = ushort.Parse(datas[1]);
                float xPos = float.Parse(datas[2]);
                float yPos = float.Parse(datas[3]);
                float zPos = float.Parse(datas[4]);
                float xRot = float.Parse(datas[5]);
                float yRot = float.Parse(datas[6]);
                float zRot = float.Parse(datas[7]);
                float xScale = float.Parse(datas[8]);
                float yScale = float.Parse(datas[9]);
                float zScale = float.Parse(datas[10]);

                string mobName = GetMonsterNametoID(id);

                GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + mobName);
                GameObject obj = GameObject.Instantiate(_obj);
                Monster monster = Monster.AddMonsterComponent(obj, id);

                monster.index = index;
                monster.id = id;
                monster.name = mobName;

                monster.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos)); // �� �ִ� ���� ����
                monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                monster.transform.localScale = new Vector3(xScale, yScale, zScale);

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

    private static string GetMonsterNametoID(ushort mobID)
    {
        string mobInfoPath = Application.dataPath + "/Resources/Tables/MonsterInfo.csv";

        MonsterInfoTableManager.LoadTable(mobInfoPath);

        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.monster_name;
        }

        throw new System.NotSupportedException(mobID + "�� �ش��ϴ� ���ʹ� �����ϴ�.");
    }
}
