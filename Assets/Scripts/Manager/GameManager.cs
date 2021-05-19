using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

// 테이블로부터 씬, 몬스터, NPC, 플레이어 기본 위치 로드
// 캐릭터 오브젝트 관리
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController controller;
    public List<Monster> monsters = new List<Monster>();
    public Slider playerHP;
    public Slider playerMP;
    public Slider playerSP;
    public TMP_Text playerLevel;

    private string villagePath;
    private string villageNPCPath;
    private string villagePlayerPath;
    private string forestPath;
    private string forestMonsterPath;
    private string forestPlayerPath;

    private void Awake()
    {
        instance = this;

        villagePath = Application.dataPath + "/Resources/Tables/Village.csv";
        villageNPCPath = Application.dataPath + "/Resources/Tables/VillageNPCPosition.csv";
        villagePlayerPath = Application.dataPath + "/Resources/Tables/VillagePlayerPosition.csv";

        forestPath = Application.dataPath + "/Resources/Tables/Forest.csv";
        forestMonsterPath = Application.dataPath + "/Resources/Tables/ForestMonsterPosition.csv";
        forestPlayerPath = Application.dataPath + "/Resources/Tables/ForestPlayerPosition.csv";
    }

    private void Start()
    {
        CreateVillage();
    }

    private void CreateVillage()
    {
        CreateScene(villagePath);
        CreatePlayer(villagePlayerPath);
        CreateNPC(villageNPCPath);
    }

    private void CreateForest()
    {
        CreateScene(forestPath);
        CreateMonster(forestMonsterPath);
        CreatePlayer(forestPlayerPath);
    }

    private void CreateMonster(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;
            GameObject parent = new GameObject("Monster");

            sr.ReadLine(); // 첫 레코드 스킵

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
                Monster monster = Monster.AddMonsterComponent(obj, id);monster.index = index;
                monster.id = id;
                monster.name = mobName;
                monster.transform.position = Utility.RayToDown(new Vector3(xPos, yPos, zPos)); // 떠 있는 현상 방지
                monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                monster.transform.localScale = new Vector3(xScale, yScale, zScale);
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
            float xPos = float.Parse(datas[0]);
            float yPos = float.Parse(datas[1]);
            float zPos = float.Parse(datas[2]);
            float xRot = float.Parse(datas[3]);
            float yRot = float.Parse(datas[4]);
            float zRot = float.Parse(datas[5]);
            float xScale = float.Parse(datas[6]);
            float yScale = float.Parse(datas[7]);
            float zScale = float.Parse(datas[8]);

            // Player Load
            GameObject _player = Resources.Load<GameObject>("Character/Player/Character_Knight_01_Black");
            GameObject player = GameObject.Instantiate(_player);
            Player playerScript = player.AddComponent<Player>();
            playerScript.name = "Character_Knight_01_Black";
            playerScript.transform.position = new Vector3(xPos, yPos, zPos);
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
            this.controller = _controller;

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

            sr.ReadLine(); // 스킵

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                string objName = datas[0];

                // 1. 공백 라인은 읽지않음
                if (objName == "")
                    continue;

                // 2. 리소스 폴더 구분
                if (objName == "Particle" || objName == "Props" || objName == "Vegetation" || objName == "Rocks" || objName == "Terrain" || objName == "Plane")
                {
                    path = objName;
                    parent = new GameObject(objName);
                    continue;
                }

                // 3. 테이블 데이터 추출
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

                // 4. 로드 & 인스턴싱
                GameObject _obj = Resources.Load<GameObject>(path + "/" + objName);
                GameObject obj = GameObject.Instantiate(_obj);

                // 5. 실제 게임오브젝트에 테이블 데이터 적용
                obj.name = objName;
                obj.transform.position = objPos;
                obj.transform.rotation = Quaternion.Euler(objAngle);
                obj.transform.localScale = objScale;

                // 6. 부모 오브젝트에 적용
                obj.transform.SetParent(parent.transform);
            }

            Debug.Log("Scene Load Completed.");
        }
    }

    private static void CreateNPC(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
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
                npcScript.transform.position = new Vector3(xPos, yPos, zPos);
                npcScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
                npcScript.transform.localScale = new Vector3(xScale, yScale, zScale);

                npcScript.transform.SetParent(parent.transform);
            }

        }

        Debug.Log("NPC Position Load Completed.");
    }
}
