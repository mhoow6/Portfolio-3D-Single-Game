using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    public void CreatePlayer(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        GameObject parent = new GameObject("Player");
        GameObject cameraArm = new GameObject("cameraArm");

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');
            byte index = byte.Parse(datas[0]);

            if (index != (byte)SceneInfoManager.instance.beforeScene)
                continue;

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
            playerScript.name = "Player";
            playerScript.transform.position = new Vector3(xPos, yPos, zPos);
            playerScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            playerScript.transform.localScale = new Vector3(xScale, yScale, zScale);

            // CustomCamera Load
            CustomCamera cameraScript = cameraArm.AddComponent<CustomCamera>();
            cameraScript.player = playerScript;
            cameraScript.transform.position = playerScript.transform.position + cameraScript.offset;
            cameraScript.transform.rotation = playerScript.transform.rotation;

            // Add Moblie Camera
            cameraScript.gameObject.AddComponent<CustomCameraMoblie>();

            // Main Camera Transform Setup
            Camera.main.transform.position = cameraScript.transform.position;
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
            PlayerController _controller = playerScript.gameObject.AddComponent<PlayerController>();
            _controller.player = playerScript;
            _controller.cameraArm = cameraScript;
            GameManager.instance.controller = _controller; // Attach at GameManger

            // Add NavMeshAgent
            player.AddComponent<NavMeshAgent>();

            // Set Parent each            
            playerScript.transform.SetParent(parent.transform);
            cameraScript.transform.SetParent(parent.transform);
            Camera.main.transform.SetParent(cameraScript.transform);
            minimapIcon.transform.SetParent(playerScript.transform);

            // local Rotation Setup
            minimapIcon.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 180f));
            Camera.main.transform.localPosition = Camera.main.transform.localPosition + cameraScript.cameraDistance;
        }

        Debug.Log("Player Load Completed.");
    }

    public void CreateScene(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        GameObject parent = null;
        string path = string.Empty;

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            string objName = datas[0];

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

            // 6. 더미 부모 게임오브젝트에 적용
            obj.transform.SetParent(parent.transform);
        }

        Debug.Log("Scene Load Completed.");
    }

    public void CreateNPC(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        GameObject parent = new GameObject("NPC");

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

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

            // Set Info From Table
            npcScript.gameObject.name = npcName;
            npcScript.index = index;
            npcScript.id = id;
            npcScript.transform.position = new Vector3(xPos, yPos, zPos);
            npcScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            npcScript.transform.localScale = new Vector3(xScale, yScale, zScale);

            // Add NavMeshAgent
            npcScript.agent = npcScript.gameObject.AddComponent<NavMeshAgent>();

            // Add GameManager
            GameManager.instance.npcs.Add(npcScript);

            npcScript.transform.SetParent(parent.transform);

        }
        Debug.Log("NPC Position Load Completed.");
    }

    public void CreateMonster(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);
        GameObject parent = new GameObject("Monster");

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

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
            monster.transform.position = new Vector3(xPos, yPos, zPos); // 떠 있는 현상 방지
            monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            monster.transform.localScale = new Vector3(xScale, yScale, zScale);

            // Add NavMeshAgent
            monster.agent = monster.gameObject.AddComponent<NavMeshAgent>();

            GameManager.instance.monsters.Add(monster);
            monster.transform.SetParent(parent.transform);
        }

        Debug.Log("Monster Load Completed.");
    }
}
