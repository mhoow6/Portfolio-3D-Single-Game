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

    private void OnEnable()
    {
        bossName.text = GameManager.instance.bossCam.boss.monster_name;
        bossHp = GameManager.instance.bossCam.boss.hp;
        bossTotalHp = GameManager.instance.bossCam.boss.spawnInfo.hp;

        bossSlider.minValue = 0;
        bossSlider.value = bossHp;
        bossSlider.maxValue = bossTotalHp;
        txtbBossTotalHp.text = bossTotalHp.ToString();
    }

    private void Update()
    {
        bossHp = GameManager.instance.bossCam.boss.hp;
        txtBossHp.text = bossHp.ToString();
        bossSlider.value = bossHp;
    }
}
