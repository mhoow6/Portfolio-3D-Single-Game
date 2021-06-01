using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlSlotManager : MonoBehaviour
{
    public List<ControlSlot> controlSlots = new List<ControlSlot>();

    [SerializeField]
    private Image skill_01_cooldownGauge;
    [SerializeField]
    private TMP_Text skill_01_cooldownDuration;
    [SerializeField]
    private Image skill_02_cooldownGauge;
    [SerializeField]
    private TMP_Text skill_02_cooldownDuration;

    private void Update()
    {
        CoolDownUpdate(
            GameManager.instance.controller.player.current_combat_skill_01_cooldown,
            PlayerInfoTableManager.playerInfo.skill_01_cooldown,
            GameManager.instance.controller.player.isPlayerUseCombatSkill01, skill_01_cooldownGauge,
            skill_01_cooldownDuration
            );

        CoolDownUpdate(
            GameManager.instance.controller.player.current_combat_skill_02_cooldown,
            PlayerInfoTableManager.playerInfo.skill_02_cooldown,
            GameManager.instance.controller.player.isPlayerUseCombatSkill02, skill_02_cooldownGauge,
            skill_02_cooldownDuration
            );
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
                textCooldown.text = string.Format("{0:0.0}", currentCooldown);
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
