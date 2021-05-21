using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController controller;
    public List<Monster> monsters = new List<Monster>();
    public List<NPC> npcs = new List<NPC>();
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
        UIPlayerStatusUpdate();
    }

    private void UIPlayerStatusUpdate()
    {
        playerHP.value = controller.player.currentHp / controller.player.hp;
        playerMP.value = controller.player.currentMp / controller.player.mp;
        playerSP.value = controller.player.currentSp / controller.player.sp;
        playerLevel.text = controller.player.level.ToString();
    }
}
