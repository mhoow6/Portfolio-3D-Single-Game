using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    public CombatManager combat;
    public InventoryManager inventory;
    public MenuManager menu;

    public Slider playerHP;
    public Slider playerMP;
    public Slider playerSP;
    public TMP_Text playerLevel;

    private void Awake()
    {
        instance = this;
    }

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
    }

    
}
