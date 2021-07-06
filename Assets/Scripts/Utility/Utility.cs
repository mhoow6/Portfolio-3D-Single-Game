using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public struct Line
{
    public float min_x;
    public float max_x;
    public float Length_x { get => max_x - min_x; }

    public float min_y;
    public float max_y;
    public float Length_y { get => max_y - min_y; }

    public float min_z;
    public float max_z;
    public float Length_z { get => max_z - min_z; }
}

public static class Utility
{
    const float SKY_HEIGHT = 20f;

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
            {
                Debug.Log("Ground hit");
                return hitinfo.point;
            }
        }
        return spawnPos;
    }

    public static IEnumerator AlphaBlending(Image origin, float finishAlpha, float lerpSpeed)
    {
        Color finish = new Color(origin.color.r, origin.color.g, origin.color.b, finishAlpha);

        if (origin.color.a < finishAlpha)
        {
            while (origin.color.a < finishAlpha - 0.01f)
            {
                yield return null;

                origin.color = Color.Lerp(origin.color, finish, Time.deltaTime * lerpSpeed);
            }

            origin.color = finish;
        }

        if (origin.color.a > finishAlpha)
        {
            while (origin.color.a > finishAlpha + 0.01f)
            {
                yield return null;

                origin.color = Color.Lerp(origin.color, finish, Time.deltaTime * lerpSpeed);
            }

            origin.color = finish;
        }
    }

    public static IEnumerator AlphaBlending(Material mat, float finishAlpha, float lerpSpeed)
    {
        StandardShaderUtils.ChangeRenderMode(mat, StandardShaderUtils.BlendMode.Fade);

        Color finish = new Color(1, 1, 1, finishAlpha);
        float time = 0f;
        float end = mat.color.a <= finishAlpha ? finishAlpha - 0.1f : finishAlpha + 0.1f;

        if (mat.color.a < finishAlpha)
        {
            while (mat.color.a < end)
            {
                yield return null;

                time += Time.deltaTime;
                mat.color = Color.Lerp(mat.color, finish, time * lerpSpeed);
            }
        }

        if (mat.color.a > finishAlpha)
        {
            while (mat.color.a > end)
            {
                yield return null;

                time += Time.deltaTime;
                mat.color = Color.Lerp(mat.color, finish, time * lerpSpeed);
            }
        }

        mat.color = finish;
        StandardShaderUtils.ChangeRenderMode(mat, StandardShaderUtils.BlendMode.Opaque);
    }

    public static IEnumerator AlphaBlending(TMP_Text tmpText, float finishAlpha, float lerpSpeed)
    {
        Color finish = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, finishAlpha);

        if (tmpText.color.a < finishAlpha)
        {
            while (tmpText.color.a < finishAlpha - 0.01f)
            {
                yield return null;

                tmpText.color = Color.Lerp(tmpText.color, finish, Time.deltaTime * lerpSpeed);
            }

            tmpText.color = finish;
        }

        if (tmpText.color.a > finishAlpha)
        {
            while (tmpText.color.a > finishAlpha + 0.01f)
            {
                yield return null;

                tmpText.color = Color.Lerp(tmpText.color, finish, Time.deltaTime * lerpSpeed);
            }

            tmpText.color = finish;
        }
    }

    public static void GetBoundsFromMeshFilter(MeshFilter mf, ref Bounds yourBound)
    {
        Vector3[] meshVertices = mf.mesh.vertices;

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

        yourBound.center = mf.gameObject.transform.position;
        yourBound.size = new Vector3(container.Length_x * mf.transform.localScale.x, container.Length_y * mf.transform.localScale.y, container.Length_z * mf.transform.localScale.z);
    }
}
