using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public LevelUpBadge badge;

    private Color startColor = new Color(1, 1, 1, 0);

    private void OnEnable()
    {
        badge.self.color = startColor;
        badge.glow.color = startColor;
        badge.txtLevel.color = startColor;
        badge.text.color = startColor;

        badge.txtLevel.text = GameManager.instance.controller.player.level.ToString();
        StartCoroutine(LevelUpAlphaBlending());
    }

    private IEnumerator LevelUpAlphaBlending()
    {
        StartCoroutine(Utility.AlphaBlending(badge.self, 1, 0.5f));
        StartCoroutine(Utility.AlphaBlending(badge.glow, 1, 0.5f));
        StartCoroutine(Utility.AlphaBlending(badge.text, 1, 0.5f));
        yield return StartCoroutine(Utility.AlphaBlending(badge.txtLevel, 1, 0.5f));
        StartCoroutine(Utility.AlphaBlending(badge.self, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(badge.glow, 0, 1f));
        StartCoroutine(Utility.AlphaBlending(badge.text, 0, 1f));
        yield return StartCoroutine(Utility.AlphaBlending(badge.txtLevel, 0, 1f));
        this.gameObject.SetActive(false);
    }
}
