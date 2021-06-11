using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ControlSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isClicked;
    public Image itemIcon;
    public TMP_Text itemCount;

    public Color originColor;
    protected Color pressedColor;
    protected const float ALPHA_80 = 0.3137255f;

    private void Awake()
    {
        originColor = itemIcon.color;
        pressedColor = new Color(itemIcon.color.r, itemIcon.color.g, itemIcon.color.b, ALPHA_80);
    }

    public virtual void PointerDown(PointerEventData eventData)
    {

    }

    public virtual void PointerUp(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;

        itemIcon.color = pressedColor;
        if (itemCount != null)
            itemCount.color = pressedColor;

        PointerDown(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        InputManager.instance.bMultiClickPrevent = false;

        itemIcon.color = originColor;
        if (itemCount != null)
            itemCount.color = originColor;

        PointerUp(eventData);
    }

    private void OnDisable()
    {
        isClicked = false;
        itemIcon.color = originColor;
    }
}
