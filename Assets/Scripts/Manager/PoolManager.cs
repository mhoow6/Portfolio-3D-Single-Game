using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public RectTransform questNode;
    public List<Image> questIcons = new List<Image>();

    private Vector2 iconSize = new Vector2(71f, 88f);

    private void Awake()
    {
        instance = this;
    }

    public Image CreateQuestIconImage()
    {
        Image questIcon = questIcons.Find(icon => (icon.gameObject.activeSelf == false) && (icon.gameObject.name == "Quest"));

        if (questIcon != null)
        {
            questIcon.gameObject.SetActive(true);
            return questIcon;
        }

        GameObject _newQuestIcon = new GameObject("Quest");

        Image newQuestIcon = _newQuestIcon.AddComponent<Image>();

        newQuestIcon.sprite = Resources.Load<Sprite>("Sprite/common_item_scroll_2");

        newQuestIcon.transform.SetParent(questNode);

        newQuestIcon.rectTransform.sizeDelta = iconSize;

        return newQuestIcon;
    }
}
