using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public void CreatePlayer(SpawnPosID criteria)
    {
        PlayerSpawnInfo currentSpawnInfo = PlayerSpawnInfoTableManager.playerSpawnInfoList.Find(spawnInfo => spawnInfo.id == (ushort)criteria);

        GameObject parent = new GameObject("Player");
        GameObject cameraArm = new GameObject("cameraArm");

        // Player Load
        GameObject _player = Resources.Load<GameObject>("Character/Player/Character_Knight_01_Black");
        GameObject player = GameObject.Instantiate(_player);
        Player playerScript = player.AddComponent<Player>();
        playerScript.name = "Player";
        playerScript.transform.position = new Vector3(currentSpawnInfo.xpos, currentSpawnInfo.ypos, currentSpawnInfo.zpos);
        playerScript.transform.rotation = Quaternion.Euler(new Vector3(currentSpawnInfo.xrot, currentSpawnInfo.yrot, currentSpawnInfo.zrot));
        playerScript.transform.localScale = new Vector3(currentSpawnInfo.xscale, currentSpawnInfo.yscale, currentSpawnInfo.zscale);

        // CustomCamera Load
        CustomCamera cameraScript = cameraArm.AddComponent<CustomCamera>();
        cameraScript.player = playerScript;
        cameraScript.transform.position = playerScript.transform.position + cameraScript.offset;
        cameraScript.transform.rotation = playerScript.transform.rotation;

        // Moblie Camera Load
        CustomCameraMoblie moblieCameraScript = cameraScript.gameObject.AddComponent<CustomCameraMoblie>();
        InputManager.instance.moblieCamera = moblieCameraScript; // Attach at InputManager

        // Alpha Wall Utilly in Camera
        AlphaWall alphaWallUtill = cameraScript.gameObject.AddComponent<AlphaWall>();
        alphaWallUtill.playerhead = playerScript.transform;
        alphaWallUtill.mainCamera = Camera.main.transform;

        // Main Camera Transform Setup
        Camera.main.transform.position = cameraScript.transform.position;
        Camera.main.transform.rotation = cameraScript.transform.rotation;

        // Inventory Camera
        GameObject _inventoryCamera = new GameObject("Inventory Camera");
        Camera inventoryCamera = _inventoryCamera.AddComponent<Camera>();
        inventoryCamera.targetTexture = Resources.Load<RenderTexture>("RenderTexture/Inventory");
        inventoryCamera.clearFlags = CameraClearFlags.SolidColor; // Transparent Background
        inventoryCamera.cullingMask = 1 << 3; // Only Rendering Layer [Ignore Minimap]
        InventoryCamera invenCameraScript = _inventoryCamera.gameObject.AddComponent<InventoryCamera>();
        HUDManager.instance.inventory.inventoryCamera = invenCameraScript; // Attach at Inventory Mananger
        invenCameraScript.gameObject.SetActive(false); // It will be actived true with Inventory On

        // Controller Load
        PlayerController _controller = playerScript.gameObject.AddComponent<PlayerController>();
        _controller.player = playerScript;
        _controller.cameraArm = cameraScript;
        GameManager.instance.controller = _controller; // Attach at GameManger

        // FootStepEffect Load
        PlayerFootstepEffect footstep = EffectManager.instance.CreateFootStepEffect();
        footstep.transform.position = playerScript.transform.position;
        playerScript.footStepEffect = footstep;
        

        // Add NavMeshAgent
        player.AddComponent<NavMeshAgent>();

        // Set Parent each            
        playerScript.transform.SetParent(parent.transform);
        cameraScript.transform.SetParent(parent.transform);
        Camera.main.transform.SetParent(cameraScript.transform);
        invenCameraScript.transform.SetParent(playerScript.transform);

        // local Setup
        Camera.main.transform.localPosition = Camera.main.transform.localPosition + cameraScript.cameraDistance;

        Debug.Log("Player Load Completed.");
    }

    public void CreateScene(string filePath, GameObject node)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);
        List<GameObject> parents = new List<GameObject>();

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
                parents.Add(parent);
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

            if (objName == "Teleport")
                GameManager.instance.teleport = obj.AddComponent<Teleport>();

            if (objName == "DragonLand")
                GameManager.instance.dragonLandFields.Add(obj.transform);

            if (objName == "Swamp")
                GameManager.instance.area = obj.AddComponent<Ground>();

            // 5. 실제 게임오브젝트에 테이블 데이터 적용
            obj.name = objName;
            obj.transform.position = objPos;
            obj.transform.rotation = Quaternion.Euler(objAngle);
            obj.transform.localScale = objScale;

            // 6. 더미 부모 게임오브젝트에 적용
            obj.transform.SetParent(parent.transform);
        }

        for (int i = 0; i < parents.Count; i++)
            parents[i].transform.SetParent(node.transform);

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

            string npcPrefab = NPCInfoTableManager.GetNPCPrefabFromID(id);

            GameObject _npc = Resources.Load<GameObject>("Character/NPC/" + npcPrefab);
            GameObject npc = GameObject.Instantiate(_npc);
            NPC npcScript = npc.AddComponent<NPC>();

            // Set Info From Table
            npcScript.gameObject.name = npcPrefab;
            npcScript.index = index;
            npcScript.id = id;
            npcScript.transform.position = new Vector3(xPos, yPos, zPos);
            npcScript.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            npcScript.transform.localScale = new Vector3(xScale, yScale, zScale);

            // Add NavMeshAgent
            npcScript.agent = npcScript.gameObject.AddComponent<NavMeshAgent>();

            // Add GameManager
            GameManager.instance.npcs.Add(npcScript);

            npcScript.transform.SetParent(parent.transform); // NPC Node
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
            string prefabName = MonsterInfoTableManager.GetMonsterPrefabNameFromID(id);

            GameObject _obj = Resources.Load<GameObject>("Character/Monster/" + prefabName);
            GameObject obj = Instantiate(_obj);
            Monster monster = Monster.AddMonsterComponent(obj, id);

            // Add Data from table
            monster.index = index;
            monster.id = id;
            monster.name = prefabName;
            monster.transform.position = new Vector3(xPos, yPos, zPos);
            monster.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
            monster.transform.localScale = new Vector3(xScale, yScale, zScale);

            // Add NavMeshAgent
            monster.agent = monster.gameObject.AddComponent<NavMeshAgent>();

            GameManager.instance.monsters.Add(monster);
            monster.transform.SetParent(parent.transform);
        }

        Debug.Log("Monster Load Completed.");
    }

    public IEnumerator GoToTheNextScene(SceneType nextScene, SpawnPosID spawnPos)
    {
        WaitForSeconds wt = new WaitForSeconds(SceneInfoManager.instance.SCENE_ROAD_DURATION);

        while (true)
        {
            yield return wt;

            if (GameManager.instance.teleport.BOUND.Intersects(GameManager.instance.controller.player._bound))
            {
                SceneInfoManager.instance.spawnPos = spawnPos;
                SceneManager.LoadScene((int)nextScene);
            }
        }
    }
}
