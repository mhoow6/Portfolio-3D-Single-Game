using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isClicked;

    [SerializeField]
    private Image background;

    private Color originColor;
    private Color pressedColor;
    private const float ALPHA_80 = 0.3137255f;

    private void Awake()
    {
        originColor = background.color;
        pressedColor = new Color(background.color.r, background.color.g, background.color.b, ALPHA_80);
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        background.color = pressedColor;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        background.color = originColor;
    }

    private void OnDisable()
    {
        isClicked = false;
        background.color = originColor;
    }
}
