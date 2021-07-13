using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public Dragon boss;

    private void OnEnable()
    {
        HUDManager.instance.ActiveUI(false);

        AudioManager.instance.StopAudioFadeOut(GameManager.instance.BGM);

        StartCoroutine(CutsceneStart());
    }


    public void CompleteCamMove()
    {
        GameManager.instance.bossCam.gameObject.SetActive(false);
        GameManager.instance.bossCam.enabled = false;
        HUDManager.instance.ActiveUI(true);
        HUDManager.instance.bFade.gameObject.SetActive(false);
        HUDManager.instance.boss_state.gameObject.SetActive(true);
    }

    IEnumerator CutsceneStart()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("BossCutscene"),
            "looktarget", boss.transform.position,
            "looktime", 0.1f,
            "time", 5f,
            "easeType", "easeInOutQuad"
            ));

        yield return null;
    }

}
