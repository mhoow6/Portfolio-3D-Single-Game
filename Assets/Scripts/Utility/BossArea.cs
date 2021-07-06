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
                    GameManager.instance.bossCam.gameObject.SetActive(true);
                    GameManager.instance.bossCam.boss.isPlayerEncounted = true;

                    yield break;
                }
            }

            yield return null;
        }
    }

}
