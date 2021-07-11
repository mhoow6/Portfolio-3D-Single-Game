using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Player player;
    public Camera mainCam;

    public Vector3 offset
    {
        get => new Vector3(0, 1.683f, 0);
    }

    public Vector3 cameraDistance
    {
        get => new Vector3(0, 0, -3.54f);
    }

    private Vector2 moveDelta;
    private float zoom;
    private float zoomResult;
    private float zoomMin;
    private float zoomMax;
    private float zoomSenstivity;
    private float zoomSpeed;

    private float originHeight;

    void Awake()
    {
        zoom = 0;
        zoomResult = 0;
        zoomMin = -1f;
        zoomMax = 1f;
        zoomSenstivity = 0.5f;
        zoomSpeed = 5.0f;

        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        LookAround();
        FollowPlayer();
    }

    private void LookAround()
    {
        moveDelta = InputManager.instance.moveDelta;

        Vector3 camAngle = transform.rotation.eulerAngles;

        float x = camAngle.x - moveDelta.y;

        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        if (x > 180f)
            x = Mathf.Clamp(x, 335f, 361f);

        transform.rotation = Quaternion.Euler(x, camAngle.y + moveDelta.x, camAngle.z);
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

    public IEnumerator jitterCamera()
    {
        Debug.Log("작동하나?");

        originHeight = mainCam.transform.position.y;

        Vector3 jitterUp = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 0.1f, mainCam.transform.position.z);
        mainCam.transform.position = jitterUp;
        yield return null;
        Vector3 jitterDown = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y - 0.2f, mainCam.transform.position.z);
        mainCam.transform.position = jitterDown;
        yield return null;
        Vector3 backOrigin = new Vector3(mainCam.transform.position.x, originHeight, mainCam.transform.position.z);
        mainCam.transform.position = backOrigin;
        yield return null;
    }
}
