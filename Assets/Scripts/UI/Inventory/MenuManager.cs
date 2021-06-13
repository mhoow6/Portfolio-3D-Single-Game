using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuIndex
{
    INVENTORY,
    SYSTEM,
    QUEST
}

public class MenuManager : MonoBehaviour
{
    public List<ControlSlot> controlSlots = new List<ControlSlot>();
}
