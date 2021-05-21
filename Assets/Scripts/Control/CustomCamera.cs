using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Player player;
    public Vector3 offset
    {
        get
        {
            return new Vector3(0, 1.683f, 0);
        }
    }

    public Vector3 cameraDistance
    {
        get
        {
            if (player.transform.forward.x >= 0)
                return new Vector3(-3.54f, 0, 0);
            else
                return new Vector3(3.54f, 0, 0);
        }
    }

    private float zoom;
    private float zoomResult;
    private float zoomMin;
    private float zoomMax;
    private float zoomSenstivity;
    private float zoomSpeed;

    void Awake()
    {
        zoom = 0;
        zoomResult = 0;
        zoomMin = -1f;
        zoomMax = 1f;
        zoomSenstivity = 0.5f;
        zoomSpeed = 5.0f;
    }

    void LateUpdate()
    {
        LookAround();
        FollowPlayer();
    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = transform.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        if (x > 180f)
            x = Mathf.Clamp(x, 335f, 361f);

        transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void FollowPlayer()
    {
        transform.position = player.gameObject.transform.position + offset + zoomOut();
    }

    private Vector3 zoomOut()
    {
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSenstivity;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
        zoomResult = Mathf.Lerp(zoomResult, zoom, Time.deltaTime * zoomSpeed);

        return transform.forward * zoomResult;
    }
}
