using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isClicked;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
