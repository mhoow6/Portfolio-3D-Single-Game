using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AI;

public class FieldManager : MonoBehaviour
{
    private string forestPath;
    private string forestMonsterPath;
    private string forestPlayerPath;
    

    private void Awake()
    {
        forestPath = Application.dataPath + "/Resources/Tables/Forest.csv";
        forestMonsterPath = Application.dataPath + "/Resources/Tables/ForestMonsterPosition.csv";
        forestPlayerPath = Application.dataPath + "/Resources/Tables/ForestPlayerPosition.csv";

        SceneInfoManager.currentScene = SceneType.Forest;
    }

    private void Start()
    {
        NavMeshManager.instance.CreateNavMesh(SceneInfoManager.currentScene);

        CreateScene(forestPath);
        CreatePlayer(forestPlayerPath);
        CreateMonster(forestMonsterPath);

        StartCoroutine(LoadSceneAsync(SceneType.Village));
    }

    private void CreateMonster(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
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
                GameObject obj = Instantiate(_obj);
                Monster monster = Monster.AddMonsterComponent(obj, id);

                // Add Data from table
                monster.index = index;
                monster.id = id;
                monster.name = mobName;
                monster.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos)); // �� �ִ� ���� ����
                monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                monster.transform.localScale = new Vector3(xScale, yScale, zScale);

                // Add NavMeshAgent
                monster.agent = monster.gameObject.AddComponent<NavMeshAgent>();

                GameManager.instance.monsters.Add(monster);
                monster.transform.SetParent(parent.transform);
            }
        }

        Debug.Log("Monster Load Completed.");
    }

    private void CreatePlayer(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Player");
            GameObject cameraArm = new GameObject("cameraArm");

            sr.ReadLine();

            string[] datas = sr.ReadLine().Split(',');
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

            // Player Load
            GameObject _player = Resources.Load<GameObject>("Character/Player/Character_Knight_01_Black");
            GameObject player = GameObject.Instantiate(_player);
            Player playerScript = player.AddComponent<Player>();
            playerScript.name = "Character_Knight_01_Black";
            playerScript.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos));
            playerScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            playerScript.transform.localScale = new Vector3(xScale, yScale, zScale);

            // Camera Load
            CustomCamera cameraScript = cameraArm.AddComponent<CustomCamera>();
            cameraScript.player = playerScript;
            cameraScript.transform.position = playerScript.transform.position + cameraScript.offset;
            cameraScript.transform.rotation = playerScript.transform.rotation;
            Camera.main.transform.position = cameraScript.transform.position + cameraScript.cameraDistance; // globalPosition x,y,z -> locaPosition z,y,x
            Camera.main.transform.rotation = cameraScript.transform.rotation;

            // Minimap Icon Load
            GameObject minimapIcon = new GameObject("MinimapIcon");
            SpriteRenderer iconRenderer = minimapIcon.AddComponent<SpriteRenderer>();
            minimapIcon.transform.position = playerScript.transform.position + new Vector3(0, 1f, 0);
            minimapIcon.transform.localScale = new Vector3(0.2f, 0.2f);
            minimapIcon.layer = 6; // Ignore MainCamera
            iconRenderer.sprite = Resources.Load<Sprite>("Sprite/player_icon");
            iconRenderer.color = Color.blue;

            // Controller Load
            PlayerController _controller = parent.AddComponent<PlayerController>();
            _controller.player = playerScript;
            _controller.cameraArm = cameraScript.transform;
            GameManager.instance.controller = _controller;

            // Add NavMeshAgent
            player.AddComponent<NavMeshAgent>();

            // Set Parent each            
            playerScript.transform.SetParent(parent.transform);
            cameraScript.transform.SetParent(parent.transform);
            Camera.main.transform.SetParent(cameraScript.transform);
            minimapIcon.transform.SetParent(playerScript.transform);

            // local Rotation Setup
            minimapIcon.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 180f));
        }

        Debug.Log("Player Load Completed.");
    }

    private void CreateScene(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line = string.Empty;
            string path = string.Empty;
            GameObject parent = null;

            sr.ReadLine(); // ��ŵ

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                string objName = datas[0];

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

    private IEnumerator LoadSceneAsync(SceneType loadScene)
    {
        SceneInfoManager.beforeScene = SceneInfoManager.currentScene;
        WaitForSeconds wt = new WaitForSeconds(SceneInfoManager.SCENE_ROAD_DURATION);

        while (true)
        {
            yield return wt;

            if (GameManager.instance.controller.player.transform.position.x < SceneInfoManager.FOREST_TO_VILLAGE_MIN_X &&
                GameManager.instance.controller.player.transform.position.z > SceneInfoManager.FOREST_TO_VILLAGE_MIN_Z &&
                GameManager.instance.controller.player.transform.position.z < SceneInfoManager.FOREST_TO_VILLAGE_MAX_Z)
                    SceneManager.LoadScene((int)loadScene);
        }
    }

    private void OnDestroy()
    {
        // Remove NavMesh Garbage
        NavMesh.RemoveAllNavMeshData();
    }
}
