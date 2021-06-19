using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameDialog : MonoBehaviour
{
    public Image dialog;

    private void OnEnable()
    {
        dialog.rectTransform.transform.position = Camera.main.WorldToScreenPoint(GameManager.instance.controller.player.dialogIcon.transform.position);
    }

    public void OnDialogOn()
    {
        HUDManager.instance.dialog.gameObject.SetActive(true);
    }
}
