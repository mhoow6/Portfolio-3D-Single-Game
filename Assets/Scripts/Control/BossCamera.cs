using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public Dragon boss;

    private void OnEnable()
    {
        HUDManager.instance.ActiveUI(false);
        HUDManager.instance.bFade.FadeIn(3f);
        StartCoroutine(CutsceneStart());
    }


    public void CompleteCamMove()
    {
        /*HUDManager.instance.ActiveUI(true);
        HUDManager.instance.bFade.FadeIn(3f);
        this.gameObject.SetActive(false);*/
    }

    IEnumerator CutsceneStart()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("BossCutscene"),
            "looktarget", boss.transform.position,
            "looktime", 0.1f,
            "time", 5f,
            "easeType", "easeInOutQuad",
            "oncomplete", "CompleteCamMove"
            ));

        yield return null;
    }

}
