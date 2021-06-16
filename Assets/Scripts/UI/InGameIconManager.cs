using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameIconManager : MonoBehaviour
{
    public RectTransform questNode;
    public List<Image> quests = new List<Image>();

    public Vector2 _questIconSize
    {
        get => questIconSize;
    }

    private Vector2 questIconSize = new Vector2(46f, 57f);
}
