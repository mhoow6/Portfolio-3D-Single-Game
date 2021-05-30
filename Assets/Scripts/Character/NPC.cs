using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character
{
    public ushort id;
    public ushort index;
    public byte npc_type;

    public NavMeshAgent agent;

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
    }
}

