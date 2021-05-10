using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadManager : ResourceManager
{
    private void Start()
    {
        // CreateScene(villagePath);
        CreateScene(forestPath);
    }

    private void CreateScene(string filePath)
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
                Debug.Log(objName);

                // 1. ���� ������ ��������
                if (objName == "")
                    continue;

                // 2. ���ҽ� ���� ����
                if (objName == "Particle" || objName == "Props" || objName == "Vegetation" || objName == "Rocks" || objName == "Terrain" || objName == "Plane")
                {
                    path = objName;
                    parent = new GameObject(objName);
                    Debug.Log($"���ҽ� ���пϷ� ({path})");
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
                Debug.Log($"���̺� ������ ����Ϸ� ({path})");

                // 4. �ε� & �ν��Ͻ�
                GameObject _obj = Resources.Load<GameObject>(path + "/" + objName);
                GameObject obj = Instantiate(_obj);
                Debug.Log($"�ν��Ͻ̿Ϸ� ({path})");

                // 5. ���� ���ӿ�����Ʈ�� ���̺� ������ ����
                obj.name = objName;
                obj.transform.position = objPos;
                obj.transform.rotation = Quaternion.Euler(objAngle);
                obj.transform.localScale = objScale;

                // 6. �θ� ������Ʈ�� ����
                obj.transform.SetParent(parent.transform);
                Debug.Log($"�θ������Ʈ�� ����Ϸ� ({path})");
            }

            Debug.Log("Scene Load Completed.");
        }
    }
}
