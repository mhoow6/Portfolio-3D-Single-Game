using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
