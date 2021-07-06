using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    public enum FadeType
    {
        IN = 0,
        OUT
    }

    public Image self;
    Color fadeInColor;
    Color fadeOutColor;

    // Start is called before the first frame update
    void Start()
    {
        fadeInColor = new Color(0, 0, 0, 1);
        fadeOutColor = new Color(0, 0, 0, 0);
    }

    public void FadeIn(float fadeTime)
    {
        StartCoroutine(FadeCoroutine(fadeTime, FadeType.IN));
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine(FadeCoroutine(fadeTime, FadeType.OUT));
    }

    IEnumerator FadeCoroutine(float fadeTime, FadeType type)
    {
        float timer = 0f;
        Color finishColor;

        switch (type)
        {
            case FadeType.IN:
                self.color = fadeOutColor;
                finishColor = fadeInColor;
                break;

            case FadeType.OUT:
                self.color = fadeInColor;
                finishColor = fadeOutColor;
                break;
        }

        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;

            self.color = Color.Lerp(self.color, fadeInColor, timer / fadeTime);

            yield return null;
        }
    }

    IEnumerator ActiveCoroutine(bool active = true)
    {
        this.gameObject.SetActive(active);
        yield return null;
    }
}
