using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public VirtualJoystick joystick;
    public CustomCameraMoblie moblieCamera;

    public Vector2 moveInput;
    public Vector2 moveDelta;
    public float zoomScale; // ¹Ì±¸Çö

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(MoveInputMoblie());
            StartCoroutine(MoveDeltaMoblie());
            return;
        }

        StartCoroutine(MoveInputPC());
        StartCoroutine(MoveDeltaPC());
        StartCoroutine(ShortcutPC());
        StartCoroutine(ShortcutMoblie());
    }

    IEnumerator MoveInputPC()
    {
        while (true)
        {
            yield return null;
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }

    IEnumerator MoveInputMoblie()
    {
        while (true)
        {
            yield return null;

            if (joystick != null)
                moveInput = joystick._inputDirection;
        }
    }

    IEnumerator MoveDeltaPC()
    {
        while (true)
        {
            yield return null;
            moveDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }

    IEnumerator MoveDeltaMoblie()
    {
        while (true)
        {
            yield return null;

            if (moblieCamera != null)
                moveDelta = moblieCamera._moveDelta;
        }
    }

    IEnumerator ShortcutPC()
    {
        while (true)
        {
            yield return null;

            // Inventory Switch
            if (Input.GetKeyDown(KeyCode.I))
            {
                bool toggle = HUDManager.instance.inventory.isInventoryOn = HUDManager.instance.inventory.isInventoryOn == false ? true : false;

                HUDManager.instance.inventory.gameObject.SetActive(toggle);
                HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(toggle);
            }
                
        }
    }

    IEnumerator ShortcutMoblie()
    {
        while (true)
        {
            yield return null;

            // Inventory
            if (HUDManager.instance.menu.controlSlots.Find(slot => slot.name == "Inventory").isClicked)
            {
                Debug.Log("Inventory On");
                HUDManager.instance.inventory.isInventoryOn = true;
                HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(true);
                HUDManager.instance.inventory.gameObject.SetActive(true);
            }
            
            if (HUDManager.instance.inventory.homeBtn.isClicked && HUDManager.instance.inventory.isInventoryOn)
            {
                Debug.Log("Inventory Off");
                HUDManager.instance.inventory.isInventoryOn = false;
                HUDManager.instance.inventory.inventoryCamera.gameObject.SetActive(false);
                HUDManager.instance.inventory.gameObject.SetActive(false);
            }
                
        }
    }
}
