using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlot : MonoBehaviour
{
    public bool isClear;
    public ushort questID;
    public string quest_name;
    public ushort target_monster_id;
    public int target_monster_hunted;
    public int target_monster_count;
    public Image questBackground;
    public TMP_Text questTitle;
    public Image questIcon;
    public QuestButton questButton;
}
