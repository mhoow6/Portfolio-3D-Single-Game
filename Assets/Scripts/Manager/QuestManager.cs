using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public QuestInfo currentQuestInfo;
    public PlayerQuestStateInfo currentQuestState;

    public ref QuestInfo _currentQuestInfo => ref currentQuestInfo;
    public ref PlayerQuestStateInfo _currentQuestState => ref currentQuestState;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Debug.Log(QuestManager.instance.currentQuestState.isPlayerAccept);
    }
}
