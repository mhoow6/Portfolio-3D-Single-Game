using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStateManager : MonoBehaviour
{
    public Slider playerHP;
    public Slider playerMP;
    public Slider playerSP;
    public TMP_Text playerLevel;

    public TMP_Text invenPlayerHP;
    public TMP_Text invenPlayerMP;
    public TMP_Text invenPlayerSP;
    public TMP_Text invenPlayerDamage;
    public TMP_Text invenPlayerLevel;
    public Slider invenPlayerExp; // �̱���

    private void Update()
    {
        PlayerStatusUpdate();
    }

    private void PlayerStatusUpdate()
    {
        if (playerHP == null)
            return;

        playerHP.value = GameManager.instance.controller.player.currentHp / GameManager.instance.controller.player.hp;
        playerMP.value = GameManager.instance.controller.player.currentMp / GameManager.instance.controller.player.mp;
        playerSP.value = GameManager.instance.controller.player.currentSp / GameManager.instance.controller.player.sp;
        playerLevel.text = GameManager.instance.controller.player.level.ToString();

        if (HUDManager.instance.inventory.isInventoryOn)
        {
            invenPlayerHP.text = GameManager.instance.controller.player.currentHp.ToString();
            invenPlayerMP.text = GameManager.instance.controller.player.currentMp.ToString();
            invenPlayerSP.text = GameManager.instance.controller.player.currentSp.ToString();
            invenPlayerDamage.text = GameManager.instance.controller.player.attack_damage.ToString();
            invenPlayerLevel.text = GameManager.instance.controller.player.level.ToString();
        }
    }
}
