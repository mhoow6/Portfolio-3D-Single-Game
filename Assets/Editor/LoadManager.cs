using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text;

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

    [MenuItem("Menu/Load/Player Position Data")]
    public static void LoadPlayerPosition()
    {
        string path = string.Empty;
        path = EditorUtility.OpenFilePanel("Load Monster", path, "csv");
        LoadPlayerPosition(path);

        Debug.Log("Player Position Load Completed.");
    }

    [MenuItem("Menu/Load/NPC Position Data")]
    public static void LoadNPCPosition()
    {
        string path = string.Empty;
        path = EditorUtility.OpenFilePanel("Load Monster", path, "csv");
        LoadNPCPosition(path);

        Debug.Log("NPC Position Load Completed.");
    }

    public static void LoadPlayerPosition(string filePath)
    {
        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(f, Encoding.UTF8))
            {
                string line = string.Empty;
                GameObject parent = new GameObject("Player");
                GameObject cameraArm = new GameObject("cameraArm");

                sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] datas = line.Split(',');
                    byte index = byte.Parse(datas[0]);
                    float xPos = float.Parse(datas[1]);
                    float yPos = float.Parse(datas[2]);
                    float zPos = float.Parse(datas[3]);
                    float xRot = float.Parse(datas[4]);
                    float yRot = float.Parse(datas[5]);
                    float zRot = float.Parse(datas[6]);
                    float xScale = float.Parse(datas[7]);
                    float yScale = float.Parse(datas[8]);
                    float zScale = float.Parse(datas[9]);

                    GameObject _player = Resources.Load<GameObject>("Character/Player/Character_Knight_01_Black");
                    GameObject player = GameObject.Instantiate(_player);
                    Player playerScript = player.AddComponent<Player>();

                    playerScript.name = "Character_Knight_01_Black";
                    playerScript.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos));
                    playerScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                    playerScript.transform.localScale = new Vector3(xScale, yScale, zScale);

                    CustomCamera cameraScript = cameraArm.AddComponent<CustomCamera>();
                    cameraScript.player = playerScript;

                    cameraScript.transform.position = playerScript.transform.position + cameraScript.offset;
                    cameraScript.transform.rotation = playerScript.transform.rotation;

                    Camera.main.transform.position = cameraScript.transform.position;
                    Camera.main.transform.rotation = cameraScript.transform.rotation;

                    playerScript.transform.SetParent(parent.transform);
                    cameraScript.transform.SetParent(parent.transform);
                    Camera.main.transform.SetParent(cameraScript.transform);

                    // Local Transform Setup
                    Camera.main.transform.localPosition = Camera.main.transform.localPosition + cameraScript.cameraDistance;

                }
            }
        }
    }

    private static void LoadMonsterPosition(string filePath)
    {
        MonsterInfoTableManager.LoadTable(filePath);

        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(f, Encoding.UTF8))
            {
                string line = string.Empty;
                GameObject parent = new GameObject("Monster");

                sr.ReadLine(); // ù ���ڵ� ��ŵ

                while ((line = sr.ReadLine()) != null)
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

                    string mobName = MonsterInfoTableManager.GetMonsterNameFromID(id);

                    GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + mobName);
                    GameObject obj = GameObject.Instantiate(_obj);
                    Monster monster = Monster.AddMonsterComponent(obj, id);

                    // monster.index = index;
                    monster.id = id;
                    monster.name = mobName;

                    monster.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos)); // �� �ִ� ���� ����
                    monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                    monster.transform.localScale = new Vector3(xScale, yScale, zScale);

                    monster.transform.SetParent(parent.transform);
                }
            }
        }
    }

    private static void LoadNPCPosition(string filePath)
    {
        NPCInfoTableManager.LoadTable(filePath);

        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(f, Encoding.UTF8))
            {
                string line = string.Empty;
                GameObject parent = new GameObject("NPC");

                sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
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

                    string npcName = NPCInfoTableManager.GetNPCNameFromID(id);

                    GameObject _npc = Resources.Load<GameObject>("Character/NPC/" + npcName);
                    GameObject npc = GameObject.Instantiate(_npc);
                    NPC npcScript = npc.AddComponent<NPC>();

                    npcScript.gameObject.name = npcName;
                    npcScript.index = index;
                    npcScript.id = id;
                    npcScript.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos));
                    npcScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                    npcScript.transform.localScale = new Vector3(xScale, yScale, zScale);

                    npcScript.transform.SetParent(parent.transform);
                }
            }
        }
    }

    private static void LoadScene(string filePath)
    {
        using (FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = string.Empty;
                string path = string.Empty;
                GameObject parent = null;

                sr.ReadLine();

                while ((line = sr.ReadLine()) != null)
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
}
