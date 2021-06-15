using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCameraMoblie : MonoBehaviour
{
    private float rightFingerId;
    private float halfScreenWidth;

    private const float HORIZONTAL_ROTATE_SPEED = 0.5f;
    private const float VERTICAL_ROTATE_SPEED = 0.2f;

    private Vector2 moveDelta;
    public Vector2 _moveDelta
    {
        get => moveDelta;
    }

    private void Awake()
    {
        rightFingerId = -1;
        halfScreenWidth = Screen.width / 2;
    }

    private void Start()
    {
        /*if (Application.platform != RuntimePlatform.Android)
            this.enabled = false;*/
    }


    private void Update()
    {
        CustomCameraMoblieUpdate();
    }

    private void CustomCameraMoblieUpdate()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (rightFingerId == -1 && t.position.x > halfScreenWidth)
                        rightFingerId = t.fingerId;
                    break;

                case TouchPhase.Moved:
                    if (t.fingerId == rightFingerId && !EventSystem.current.IsPointerOverGameObject(i))
                    {
                        if (Mathf.Abs(t.deltaPosition.x) > Mathf.Abs(t.deltaPosition.y)) // 수평이동 값이 수직이동 값보다 클 경우
                            moveDelta = new Vector2(t.deltaPosition.x * HORIZONTAL_ROTATE_SPEED, 0);
                        else if (Mathf.Abs(t.deltaPosition.x) < Mathf.Abs(t.deltaPosition.y))
                            moveDelta = new Vector2(0, t.deltaPosition.y * VERTICAL_ROTATE_SPEED);
                    }
                    break;

                case TouchPhase.Stationary:
                    if (t.fingerId == rightFingerId)
                        moveDelta = Vector2.zero;
                    break;

                case TouchPhase.Ended:
                    if (t.fingerId == rightFingerId)
                        rightFingerId = -1;
                    break;

                case TouchPhase.Canceled:
                    if (t.fingerId == rightFingerId)
                        rightFingerId = -1;
                    break;
            }
        }
    }
}
