using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeadWindowManager : MonoBehaviour
{
    public Image background; // 0 ~ 180
    public DeadBanner banner;
    public Image goVillageBtn_image;
    public TMP_Text goVillageBtn_text;
    public bool isDeadWindowOn;

    private Color backgroundOriginColor = new Color(0.103f, 0.081f, 0.081f, 0);
    private Color originColor = new Color(1, 1, 1, 0);

    private const float backgroundFinishAlpha = 0.7f;
    private const float bannerFinishAlpha = 1f;
    private const float COLOR_LERF_SPEED = 1f;

    public void GoVillage()
    {
        SceneInfoManager.instance.spawnPos = SpawnPosID.VILLAGE_START;
        SceneManager.LoadScene((int)SceneType.Village);
        HUDManager.instance.system.SaveGame();
    }

    private void OnEnable()
    {
        isDeadWindowOn = true;
        StartCoroutine(DeadWindowAlphaBlending());
    }

    private void OnDisable()
    {
        isDeadWindowOn = false;
        background.color = backgroundOriginColor;
        banner.self.color = originColor;
        banner.self_text.color = originColor;
        goVillageBtn_image.color = originColor;
        goVillageBtn_text.color = originColor;
    }

    private IEnumerator DeadWindowAlphaBlending()
    {
        StartCoroutine(Utility.AlphaBlending(background, backgroundFinishAlpha, COLOR_LERF_SPEED));
        StartCoroutine(Utility.AlphaBlending(banner.self, bannerFinishAlpha, COLOR_LERF_SPEED));
        yield return StartCoroutine(Utility.AlphaBlending(banner.self_text, bannerFinishAlpha, COLOR_LERF_SPEED));
        yield return StartCoroutine(ButtonAlphaBlending());
    }

    private IEnumerator ButtonAlphaBlending()
    {
        StartCoroutine(Utility.AlphaBlending(goVillageBtn_image, bannerFinishAlpha, COLOR_LERF_SPEED));
        StartCoroutine(Utility.AlphaBlending(goVillageBtn_text, bannerFinishAlpha, COLOR_LERF_SPEED));
        yield return null;
    }
}
