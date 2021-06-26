using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private Bounds selfBound;
    public Bounds BOUND { get => selfBound; }
    private MeshFilter meshFilter;
    //public MeshRenderer render;

    private void Start()
    {
        //render = GetComponent<MeshRenderer>();
        //render.enabled = false;

        meshFilter = GetComponent<MeshFilter>();
        Vector3[] meshVertices = meshFilter.mesh.vertices;

        Line container;
        container.min_x = meshVertices[0].x;
        container.max_x = meshVertices[0].x;
        container.min_y = meshVertices[0].y;
        container.max_y = meshVertices[0].y;
        container.min_z = meshVertices[0].z;
        container.max_z = meshVertices[0].z;

        foreach (Vector3 vertex in meshVertices)
        {
            if (vertex.x < container.min_x)
                container.min_x = vertex.x;

            if (vertex.x > container.max_x)
                container.max_x = vertex.x;

            if (vertex.y < container.min_y)
                container.min_y = vertex.y;

            if (vertex.y > container.max_y)
                container.max_y = vertex.y;

            if (vertex.z < container.min_z)
                container.min_z = vertex.z;

            if (vertex.z > container.max_z)
                container.max_z = vertex.z;
        }

        selfBound.center = transform.position;
        selfBound.size = new Vector3(container.Length_x, container.Length_y, container.Length_z);
    }

}
