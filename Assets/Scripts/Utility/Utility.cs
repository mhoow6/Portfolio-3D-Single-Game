using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    const float SKY_HEIGHT = 100f;
    public static Vector3 RayToDown(Vector3 spawnPos)
    {
        Vector3 origin = spawnPos;
        origin.y += SKY_HEIGHT;
        RaycastHit hitinfo;

        int layermask = 1 << 2; // If Object had 2nd Layer, Raycast is ignored.
        layermask = ~layermask;

        if (Physics.Raycast(origin, Vector3.down, out hitinfo, Mathf.Infinity, layermask))
        {
            if (hitinfo.collider.CompareTag("Ground"))
                return hitinfo.point;
        }
        return spawnPos;
        
    }
}
