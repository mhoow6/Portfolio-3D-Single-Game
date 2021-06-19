using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameIcons : MonoBehaviour
{
    public List<Image> questIcons = new List<Image>();
    public InGameDialog dialogIcon;

    public Vector2 _questIconSize
    {
        get => questIconSize;
    }

    private Vector2 questIconSize = new Vector2(46f, 57f);
}
