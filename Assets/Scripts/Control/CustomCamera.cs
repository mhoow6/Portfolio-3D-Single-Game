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

    private float originZ;
    private float originY;
    private float originX;

    private const float JITTER_SENSIVITY = 0.007f;
    private const float JITTER_MAX = 0.6f;
    private WaitForSeconds jitter_cooldown = new WaitForSeconds(0.1f);
    private WaitForSeconds jitter_wait = new WaitForSeconds(0.05f);
    public bool isJitter;

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

    public IEnumerator jitterCamera(float damage)
    {
        float normalized = damage * JITTER_SENSIVITY;
        normalized = (float)(normalized > JITTER_MAX ? JITTER_MAX : normalized);
        isJitter = true;

        originX = mainCam.transform.localPosition.x;
        originZ = mainCam.transform.localPosition.z;
        
        Vector3 jitterUp = new Vector3(mainCam.transform.localPosition.x + normalized, mainCam.transform.localPosition.y, mainCam.transform.localPosition.z + normalized);
        mainCam.transform.localPosition = jitterUp;
        yield return jitter_wait;

        Vector3 backOrigin = new Vector3(originX, mainCam.transform.localPosition.y, originZ);
        mainCam.transform.localPosition = backOrigin;
        yield return jitter_cooldown;

        isJitter = false;
        yield return null;
    }
}
