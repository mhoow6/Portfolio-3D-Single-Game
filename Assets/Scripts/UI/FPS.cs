using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FPS : MonoBehaviour 
{
    public TMP_Text fpsText;
    float deltaTime = 0.0f;
    const float CHECKING_SPEED = 0.2f;

    private void Start()
    {
        StartCoroutine(FPSCalculator(CHECKING_SPEED));
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private IEnumerator FPSCalculator(float checkingSpeed)
    {
        WaitForSeconds wt = new WaitForSeconds(checkingSpeed);

        while (true)
        {
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} fps", fps);

            fpsText.text = text;

            yield return wt;
        }
    }
}
