using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public TMP_Text npcName;
    public TMP_Text npcChat;
    public Button skipBtn;
    public RectTransform questBtnNode;
    public bool isDialogOn;

    private void OnEnable()
    {
        InputManager.instance.joystick.gameObject.SetActive(false);
        HUDManager.instance.combat.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InputManager.instance.joystick.gameObject.SetActive(true);
        HUDManager.instance.combat.gameObject.SetActive(true);
    }

    public void OnSkip()
    {

    }

    public void OnQuestAccept()
    {

    }

    public void OnQuestDecline()
    {

    }
}