using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField, Range(10, 250)]
    private float leverRange;

    private Vector2 inputDirection;
    private bool isInput;

    private void Update()
    {
        if (isInput && Application.platform == RuntimePlatform.Android)
            GameManager.instance.controller._moveInput = inputDirection;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
        GameManager.instance.controller._moveInput = Vector2.zero;
    }

    private void ControlJoyStickLever(PointerEventData eventData)
    {
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;

        inputDirection = inputVector / leverRange;
    }
}
