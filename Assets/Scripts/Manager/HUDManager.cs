using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public Slider playerHP;
    public Slider playerMP;
    public Slider playerSP;
    public TMP_Text playerLevel;

    public Image skill_01_cooldownGauge;
    public TMP_Text skill_01_cooldownDuration;

    public Image skill_02_cooldownGauge;
    public TMP_Text skill_02_cooldownDuration;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        PlayerStatusUpdate();

        CoolDownUpdate(
            GameManager.instance.controller.player.current_combat_skill_01_cooldown,
            PlayerInfoTableManager.playerInfo.skill_01_cooldown,
            GameManager.instance.controller.player.isPlayerUseCombatSkill01,
            skill_01_cooldownGauge,
            skill_01_cooldownDuration
            );
    }


    private void PlayerStatusUpdate()
    {
        if (playerHP == null)
            return;

        playerHP.value = GameManager.instance.controller.player.currentHp / GameManager.instance.controller.player.hp;
        playerMP.value = GameManager.instance.controller.player.currentMp / GameManager.instance.controller.player.mp;
        playerSP.value = GameManager.instance.controller.player.currentSp / GameManager.instance.controller.player.sp;
        playerLevel.text = GameManager.instance.controller.player.level.ToString();
    }

    private void CoolDownUpdate(float currentCooldown, float originCooldown, bool flag, Image gauge, TMP_Text textCooldown)
    {
        if (flag)
        {
            if (currentCooldown != 0)
            {
                gauge.gameObject.SetActive(true);
                textCooldown.gameObject.SetActive(true);
                gauge.fillAmount = currentCooldown / originCooldown;
                textCooldown.text = currentCooldown.ToString();
            }

            if (currentCooldown == 0)
            {
                gauge.gameObject.SetActive(false);
                textCooldown.gameObject.SetActive(false);
            }
        }
        else
            return;
    }
}
