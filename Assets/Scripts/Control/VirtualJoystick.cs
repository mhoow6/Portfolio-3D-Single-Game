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

    [SerializeField]
    private Vector2 inputDirection;

    public Vector2 _inputDirection
    {
        get => inputDirection;
    }

    private void Start()
    {
        InputManager.instance.joystick = this;

        if (Application.platform != RuntimePlatform.Android)
            this.enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        inputDirection = Vector2.zero;
    }

    private void ControlJoyStickLever(PointerEventData eventData)
    {
        Vector2 inputPos = eventData.position - rectTransform.anchoredPosition;
        Vector2 inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;

        inputDirection = inputVector / leverRange;
    }
}
