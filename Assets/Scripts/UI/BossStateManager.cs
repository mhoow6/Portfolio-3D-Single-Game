using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossStateManager : MonoBehaviour
{
    public Slider bossSlider;
    public TMP_Text bossName;
    public TMP_Text txtBossHp;
    private float bossHp;
    public TMP_Text txtbBossTotalHp;
    private float bossTotalHp;

    const float DISABLE_TIME = 1f;

    private void OnEnable()
    {
        bossName.text = GameManager.instance.bossCam.boss.monster_name;
        bossHp = GameManager.instance.bossCam.boss.hp;
        bossTotalHp = GameManager.instance.bossCam.boss.spawnInfo.hp;

        bossSlider.minValue = 0;
        bossSlider.value = bossHp;
        bossSlider.maxValue = bossTotalHp;
        txtbBossTotalHp.text = bossTotalHp.ToString();

        GameManager.instance.BGM = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_FOREST, AudioCondition.DRAGON_BATTLE_START));
        GameManager.instance.BGM.loop = true;
        AudioManager.instance.PlayAudioFadeIn(GameManager.instance.BGM, AudioManager.instance._DRAGON_BATTLE_SOUND);
    }

    private void Update()
    {
        bossHp = GameManager.instance.bossCam.boss.hp;
        txtBossHp.text = bossHp.ToString();
        bossSlider.value = bossHp;
    }

    public void DisableSoon()
    {
        AudioManager.instance.StopAudioFadeOut(GameManager.instance.BGM);

        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        float timer = 0f;

        while (timer < DISABLE_TIME)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        DisableEnter();
    }

    void DisableEnter()
    {
        GameManager.instance.BGM = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_FOREST, AudioCondition.SCENE_AWAKE));
        GameManager.instance.BGM.loop = true;
        AudioManager.instance.PlayAudioFadeIn(GameManager.instance.BGM, AudioManager.instance._FOREST_SOUND);

        this.gameObject.SetActive(false);
    }
}
