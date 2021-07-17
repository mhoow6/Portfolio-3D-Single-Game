using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 주의점
 * 1. 미니맵은 반드시 x,z의 양의 축 방향, Isolation으로 찍어야 함.
 * 2. ScrollRect의 normalizedPosition을 중앙점(0.5f,0.5f)으로 맞출 때 미니맵에서 중앙이 되는데, 미니맵의 중앙을 가리키는 실제 게임 상에서의 좌표도 같이 얻어와야 함
 * 3. [Content 1:1 비율] 맵 사이즈는 (0,0)에서부터 반지름 값을 가져오고, 반드시 캡쳐한 맵 양만큼만 얻어올 것.
 *          만약 정확히 맵 사이즈를 쟀는데 값이 안 맞는다? 미니맵을 잘 못 캡쳐해서 미니맵만큼 안 찍은거임
 * 4. [Content 2:1 비율] 맵 사이즈에서 x축만 값 그대로 가져와
 */
public class ScrollMinimap : MonoBehaviour
{
    public ScrollRect self;

    public Vector3 START_POS; // 미니맵 중앙좌표가 가리키는 실제 게임에서의 좌표
    public Vector2 MAP_SIZE;
    public Vector3 originPos;

    Vector2 NORMAL_POS = new Vector2(0.5f, 0.5f); // 미니맵의 중앙

    private void Start()
    {
        switch (SceneInfoManager.instance.currentScene)
        {
            case SceneType.Village:
                START_POS = SceneInfoManager.instance.VILLAGE_MINIMAP_CENTER;
                MAP_SIZE = SceneInfoManager.instance.VILLAGE_MAP_RADIUS;
                break;
            case SceneType.Forest:
                START_POS = SceneInfoManager.instance.FOREST_MINIMAP_CENTER;
                MAP_SIZE = SceneInfoManager.instance.FOREST_MAP_RADIUS;
                break;
        }

        originPos = START_POS;

        self.normalizedPosition = NORMAL_POS;
        MinimapUpdate();

    }

    private void Update()
    {
        if (InputManager.instance.moveInput.magnitude != 0)
            MinimapUpdate();

        if (Input.GetKeyDown(KeyCode.Return))
            CenterEstimate();

        if (Input.GetKeyDown(KeyCode.Backspace))
            self.normalizedPosition = new Vector2(0.5f, 0.5f);

    }

    private void MinimapUpdate()
    {
        float fDeltax = GameManager.instance.controller.player.transform.position.x - START_POS.x;
        float fDeltay = GameManager.instance.controller.player.transform.position.z - START_POS.z;

        float ratiox = fDeltax / MAP_SIZE.x;
        float ratioy = fDeltay / MAP_SIZE.y;
        NORMAL_POS.Set(ratiox, ratioy);

        self.normalizedPosition = self.normalizedPosition + NORMAL_POS;

        START_POS = GameManager.instance.controller.player.transform.position;
    }

    private void CenterEstimate()
    {
        float fDeltax = GameManager.instance.controller.player.transform.position.x - originPos.x;
        float fDeltay = GameManager.instance.controller.player.transform.position.z - originPos.z;

        float ratiox = fDeltax / MAP_SIZE.x;
        float ratioy = fDeltay / MAP_SIZE.y;
        NORMAL_POS.Set(ratiox, ratioy);

        self.normalizedPosition = self.normalizedPosition + NORMAL_POS;
    }
}
