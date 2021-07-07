using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : Teleport
{
    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        Utility.GetBoundsFromMeshFilter(meshFilter, ref selfBound);

        StartCoroutine(DetectPlayer());
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            if (GameManager.instance.controller.player != null)
            {
                if (selfBound.Intersects(GameManager.instance.controller.player._bound))
                {
                    HUDManager.instance.bFade.gameObject.SetActive(true);
                    yield return HUDManager.instance.bFade.StartCoroutine(HUDManager.instance.bFade.FadeCoroutine(3f, BlackFade.FadeType.IN));
                    GameManager.instance.bossCam.gameObject.SetActive(true);
                    GameManager.instance.bossCam.enabled = true;
                    GameManager.instance.bossCam.boss.isPlayerEncounted = true;
                    yield return HUDManager.instance.bFade.StartCoroutine(HUDManager.instance.bFade.FadeCoroutine(3f, BlackFade.FadeType.OUT));
                    yield break;
                }
            }

            yield return null;
        }
    }
}
