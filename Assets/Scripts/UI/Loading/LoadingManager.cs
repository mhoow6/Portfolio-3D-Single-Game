using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public Image sceneBackground;
    public Image completedLoadingBackground;
    public TMP_Text txt_loadingGauge;
    public Slider loadingGauge;
    public bool isLoadingOn;

    private const float LOADING_SPEED = 0.5f;
    private const float SCENE_LOAD_CHECK_DURATION = 2f;
    private const float FADE_SPEED = 2f;

    private void OnEnable()
    {
        isLoadingOn = true;
    }

    private void Start()
    {
        StartCoroutine(loading(LOADING_SPEED));
    }

    private IEnumerator loading(float loadingSpeed)
    {
        WaitForSeconds wt = new WaitForSeconds(SCENE_LOAD_CHECK_DURATION);
            
        while (loadingGauge.value <= loadingGauge.maxValue - 0.05f)
        {
            yield return null;

            loadingGauge.value = Mathf.Lerp(loadingGauge.value, loadingGauge.maxValue, Time.deltaTime * loadingSpeed);
            txt_loadingGauge.text = "Loading..." + string.Format("{0:0}%", loadingGauge.value * 100);
        }

        while (!SceneInfoManager.instance.isSceneLoadCompleted)
        {
            yield return wt;
        }

        loadingGauge.value = 1f;

        completedLoadingBackground.gameObject.SetActive(true);
        sceneBackground.gameObject.SetActive(false);
        txt_loadingGauge.gameObject.SetActive(false);
        loadingGauge.gameObject.SetActive(false);
        isLoadingOn = false;

        StartCoroutine(loadingCompleted());
    }

    private IEnumerator loadingCompleted()
    {
        yield return StartCoroutine(Utility.AlphaBlending(completedLoadingBackground, 0, FADE_SPEED));
        this.gameObject.SetActive(false);
        HUDManager.instance.place.gameObject.SetActive(true);
        Debug.Log("로딩 종료");
    }
}
