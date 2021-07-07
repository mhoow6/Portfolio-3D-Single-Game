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
    public Color _fadeInColor { get => fadeInColor; }
    Color fadeInColor;
    public Color _fadeOutColor { get => fadeOutColor; }
    Color fadeOutColor;
    public Color _finishColor { get => finishColor; }
    Color finishColor;

    float fadeSmoothTime;

    private void Awake()
    {
        fadeSmoothTime = 0.05f;
        fadeInColor = new Color(0, 0, 0, 1);
        fadeOutColor = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
    }

    public IEnumerator FadeCoroutine(float fadeTime, FadeType type)
    {
        float timer = 0f;

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


        while (timer < fadeTime)
        {
            timer += Time.deltaTime;

            self.color = Color.Lerp(self.color, finishColor, timer * fadeSmoothTime / fadeTime);

            yield return null;

        }

        self.color = finishColor;
    }
}
