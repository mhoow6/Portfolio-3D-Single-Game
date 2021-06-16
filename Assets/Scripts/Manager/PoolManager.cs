using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Image CreateQuestIconImage()
    {
        Image questIcon = HUDManager.instance.inGame.quests.Find(icon => (icon.gameObject.activeSelf == false) && (icon.gameObject.name == "Quest"));

        if (questIcon != null)
        {
            questIcon.gameObject.SetActive(true);
            return questIcon;
        }

        GameObject _newQuestIcon = new GameObject("Quest");

        Image newQuestIcon = _newQuestIcon.AddComponent<Image>();

        newQuestIcon.sprite = Resources.Load<Sprite>("Sprite/common_item_scroll_2");

        newQuestIcon.transform.SetParent(HUDManager.instance.inGame.questNode.transform);

        newQuestIcon.rectTransform.sizeDelta = HUDManager.instance.inGame._questIconSize;

        return newQuestIcon;
    }
}
