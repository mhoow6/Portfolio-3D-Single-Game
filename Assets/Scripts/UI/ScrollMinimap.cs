using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* ������
 * 1. �̴ϸ��� �ݵ�� x,z�� ���� �� ����, Isolation���� ���� ��.
 * 2. ScrollRect�� normalizedPosition�� �߾���(0.5f,0.5f)���� ���� �� �̴ϸʿ��� �߾��� �Ǵµ�, �̴ϸ��� �߾��� ����Ű�� ���� ���� �󿡼��� ��ǥ�� ���� ���;� ��
 * 3. [Content 1:1 ����] �� ������� (0,0)�������� ������ ���� ��������, �ݵ�� ĸ���� �� �縸ŭ�� ���� ��.
 *          ���� ��Ȯ�� �� ����� ��µ� ���� �� �´´�? �̴ϸ��� �� �� ĸ���ؼ� �̴ϸʸ�ŭ �� ��������
 * 4. [Content 2:1 ����] �� ������� x�ุ �� �״�� ������
 */
public class ScrollMinimap : MonoBehaviour
{
    public ScrollRect self;

    Vector3 START_POS; // ���� ���ӿ����� �߾�
    Vector2 MAP_SIZE;
    Vector2 NORMAL_POS = new Vector2(0.5f, 0.5f); // ���� ���ӿ����� �߾�

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

        self.normalizedPosition = NORMAL_POS;
        MinimapUpdate();

    }

    private void Update()
    {
        if (InputManager.instance.moveInput.magnitude != 0)
            MinimapUpdate();
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
}
