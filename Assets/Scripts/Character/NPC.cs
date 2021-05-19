using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public ushort id;
    public ushort index;
    public byte npc_type;

    private void Awake()
    {
        hp = 0;
        attack_damage = 0;
        attack_distance = 0;
        attack_angle = 0;
        walk_speed = 0;
    }

    private void Start()
    {
        foreach (NpcInfo npcinfo in NPCInfoTableManager.npcInfoList)
        {
            if (npcinfo.id == id)
            {
                npc_type = npcinfo.npc_type;
                return;
            }
        }
    }
}

