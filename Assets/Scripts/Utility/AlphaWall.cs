using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWall : MonoBehaviour
{
    public Transform playerhead;
    public Transform mainCamera;

    [SerializeField]
    private List<GameObject> Alphalist = new List<GameObject>();
    [SerializeField]
    private List<GameObject> Recoverlist = new List<GameObject>();

    private const float SCAN_DURATION = 0.3f;

    private void Start()
    {
        StartCoroutine(Transparent(SCAN_DURATION));
    }

    private IEnumerator Transparent(float scanDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(scanDuration);

        while (true)
        {
            yield return wt;

            Vector3 origin = mainCamera.position;
            Vector3 dir = playerhead.position - mainCamera.position;

            RaycastHit[] hits = Physics.RaycastAll(origin, dir.normalized, playerhead.position.x - mainCamera.position.x);

            // 1. No hits -> Empty list
            if (hits.Length == 0)
            {
                EmptyAlphaList();
                yield return null;
            }
            // 2. Raycast-hit -> Alpha list
            else
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.tag != "Ground")
                        AddAlphaList(hits[i].collider.gameObject);
                }
                    
            }

            // 3. Find No Raycast-hit element
            for (int i = 0; i < Alphalist.Count; i++)
            {
                GameObject tmp = null;

                for (int j = 0; j < hits.Length; j++)
                {
                    try
                    {
                        if (Alphalist[i].name == hits[j].collider.gameObject.name)
                            tmp = hits[j].collider.gameObject;
                    }
                    catch (System.IndexOutOfRangeException e)
                    {
                        Debug.Log("i index = " + i);
                        Debug.Log("j index = " + j);
                    }
                }

                // 4. Find No Raycast-hit element, then tmp is null
                if (tmp == null)
                {
                    // Let's find No Raycast-hit element in Recover list
                    GameObject recoverObj = Recoverlist.Find(obj => obj.name == Alphalist[i].name);

                    // No Raycast-hit element found.
                    if (recoverObj != null)
                        continue;

                    // No Raycast-hit element -> Recover list
                    AddRecoverList(Alphalist[i]);
                }
            }

            // 5. Remove Object in Recoverlist to Alphalist
            GameObject[] recoverArray = Recoverlist.ToArray();
            for (int i = 0; i < recoverArray.Length; i++)
            {
                try
                {
                    GameObject findObj = Alphalist.Find(o => (o.name == Recoverlist[i].name));

                    if (findObj != null)
                        Alphalist.Remove(findObj);

                }
                catch (System.ArgumentOutOfRangeException)
                {
                    Debug.Log("i index = " + i);
                }
            }

            Recoverlist.Clear();
        }
    }

    private void AddAlphaList(GameObject hitObject)
    {
        // Same Object is not add in list
        GameObject alphaObj = FindAlphaList(hitObject);

        // 1. Get hit object's mesh renderer
        MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();

        if (alphaObj == null && meshRenderer != null)
        {
            Alphalist.Add(hitObject);

            // 2. Change material rendering mode
            StandardShaderUtils.ChangeRenderMode(meshRenderer.material, StandardShaderUtils.BlendMode.Fade);

            // 3. Change Alpha
            Color col = meshRenderer.material.color;
            col.a = 0.2f;
            hitObject.GetComponent<MeshRenderer>().material.color = col;
        }
    }

    private GameObject FindAlphaList(GameObject obj)
    {
        GameObject findObj = Alphalist.Find(o => (o.name == obj.name));
        return findObj;
    }

    private void EmptyAlphaList()
    {
        for (int i = 0; i < Alphalist.Count; i++)
        {
            // 1. Get Alphalist element's mesh renderer
            MeshRenderer meshRenderer = Alphalist[i].GetComponent<MeshRenderer>();

            // 2. Change Alpha
            Color color = meshRenderer.material.color;
            color.a = 1f;
            meshRenderer.material.color = color;

            // 3. Change material rendering mode
            StandardShaderUtils.ChangeRenderMode(meshRenderer.material, StandardShaderUtils.BlendMode.Opaque);
        }
        Alphalist.Clear();
    }

    private void AddRecoverList(GameObject alphaObject)
    {
        for (int i = 0; i < Alphalist.Count; i++)
        {
            // 1. Get Alphalist element's mesh renderer
            MeshRenderer meshRenderer = alphaObject.GetComponent<MeshRenderer>();

            // 2. Change Alpha
            Color color = meshRenderer.material.color;
            color.a = 1f;
            meshRenderer.material.color = color;

            // 3. Change material rendering mode
            StandardShaderUtils.ChangeRenderMode(meshRenderer.material, StandardShaderUtils.BlendMode.Opaque);

            // 4. Add Recoverlist
            Recoverlist.Add(Alphalist[i]);
        }
    }
}
