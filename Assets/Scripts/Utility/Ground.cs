using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Teleport
{
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Utility.GetBoundsFromMeshFilter(meshFilter, ref selfBound);
    }

}
