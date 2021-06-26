using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AwardCheckManager : MonoBehaviour
{
    public TMP_Text questName;
    public AwardItem award;
    public bool isAwardCheckOn;

    private void OnEnable()
    {
        isAwardCheckOn = true;
    }

    private void OnDisable()
    {
        isAwardCheckOn = false;
    }

    public void Check()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowItem(string questname, ushort awardItemID, byte awardItemType, int awardItemCount)
    {
        this.gameObject.SetActive(true);

        questName.text = questname;
        award.itemCount.text = awardItemCount.ToString();

        switch (awardItemType)
        {
            case (byte)ItemType.EQUIPMENT:
                award.itemIcon.sprite = Resources.Load<Sprite>("Sprite/" + WeaponInfoTableManager.GetPrefabNameFromWeaponID(awardItemID));
                award.itemCount.gameObject.SetActive(false);
                award.itemName.text = WeaponInfoTableManager.GetWeaponInfoFromWeaponID(awardItemID).weapon_name.ToString();
                break;
            case (byte)ItemType.CONSUME:
                award.itemIcon.sprite = Resources.Load<Sprite>("Sprite/" + ConsumeInfoTableManager.GetPrefabNameInfoFromID(awardItemID));
                award.itemName.text = ConsumeInfoTableManager.GetConsumeItemInfoFromID(awardItemID).item_name.ToString();
                break;
        }
    }
}
