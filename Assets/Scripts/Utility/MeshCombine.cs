using UnityEngine;
using System.Collections;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombine : MonoBehaviour
{
    [SerializeField]
    MeshFilter[] meshFilters;

    [SerializeField]
    int childCount;

    void Start()
    {
        childCount = transform.childCount;
        meshFilters = new MeshFilter[childCount];

        ChildrenMeshCombine();
    }

    private void GetChildrenMeshFilter()
    {
        for(int i = 0; i < childCount; i++)
        {
            MeshFilter childMeshFilter = transform.GetChild(i).GetComponent<MeshFilter>();
            meshFilters[i] = childMeshFilter;
        }
    }

    private void ChildrenMeshCombine()
    {
        GetChildrenMeshFilter();

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
    }
}