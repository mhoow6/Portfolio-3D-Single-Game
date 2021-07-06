using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Bounds selfBound;
    public Bounds BOUND { get => selfBound; }
    protected MeshFilter meshFilter;
    //public MeshRenderer render;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Utility.GetBoundsFromMeshFilter(meshFilter, ref selfBound);
    }

}
