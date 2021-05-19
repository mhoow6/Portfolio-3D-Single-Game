using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Vector3 newPosition;
    

    private void LateUpdate()
    {
        newPosition = GameManager.instance.controller.player.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
