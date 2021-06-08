using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TapSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text menuText;
    public Image originMenuIcon;
    public Image focuseMenuIcon;
    public bool isClicked;

    private Vector2 tabFocusAnchorPos = new Vector2(-1, -48.52002f);

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;

        // Prev TabFocus handle
        if (HUDManager.instance.inventory.tapMenu.tabFocus.transform.parent != this.transform)
        {
            TapSlot currentTabFocuseHolder = HUDManager.instance.inventory.tapMenu.tabs.Find(tap => tap.transform == HUDManager.instance.inventory.tapMenu.tabFocus.transform.parent);

            if (currentTabFocuseHolder.menuText != null)
            {
                Color tempColor = new Color();
                ColorUtility.TryParseHtmlString("#FFFFFF", out tempColor);
                currentTabFocuseHolder.menuText.color = tempColor;
            }
            else
            {
                currentTabFocuseHolder.originMenuIcon.gameObject.SetActive(true);
                currentTabFocuseHolder.focuseMenuIcon.gameObject.SetActive(false);
            }
            
        }

        // TabFocus Setup
        if (menuText != null)
        {
            Color tempColor = new Color();
            ColorUtility.TryParseHtmlString("#F6E19C", out tempColor);
            menuText.color = tempColor;
        }
        else
        {
            originMenuIcon.gameObject.SetActive(false);
            focuseMenuIcon.gameObject.SetActive(true);
        }

        // Get TabFocus Transform into here
        HUDManager.instance.inventory.tapMenu.tabFocus.transform.SetParent(this.transform);
        HUDManager.instance.inventory.tapMenu.tabFocus.rectTransform.anchoredPosition = tabFocusAnchorPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
