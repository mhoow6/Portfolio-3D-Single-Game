using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCamera : MonoBehaviour
{
    public Vector3 BasicOffset
    {
        get
        {
            return new Vector3(0, 1f, 1.8f);
        }
    }

    private void LateUpdate()
    {
        transform.localPosition = BasicOffset;
        transform.forward = -GameManager.instance.controller.player.transform.forward;
    }
}
