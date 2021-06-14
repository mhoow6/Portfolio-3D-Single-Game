using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPC : Character
{
    public ushort id;
    public ushort index;
    public byte npc_type;

    public NavMeshAgent agent;
    //public Image questIcon;
    //public Transform head;
    private const int AGENT_PRIORITY = 49;
    
    private void Start()
    {
        foreach (NpcInfo npcinfo in NPCInfoTableManager.npcInfoList)
        {
            if (npcinfo.id == id)
            {
                npc_type = npcinfo.npc_type;
                agent.avoidancePriority = AGENT_PRIORITY;
                return;
            }
        }

        bound = GetBoundFromSkinnedMeshRenderer(this).Value;
    }

    /*private void QuestIconUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(head.position);
        questIcon.transform.position = screenPos;
        Debug.Log($"{questIcon.transform.position}");
    }*/
}

