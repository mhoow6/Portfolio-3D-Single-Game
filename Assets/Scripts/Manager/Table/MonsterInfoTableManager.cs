using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct MonsterInfo
{
    public ushort id;
    public string prefab_name;
    public string monster_name;
    public byte monster_type;
    public float hp;
    public float attack_damage;
    public float attack_distance;
    public float attack_angle;
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
    public float skill_2_damage;
    public float skill_2_distance;
    public float skill_2_angle;
    public float skill_3_damage;
    public float skill_3_distance;
    public float skill_3_angle;
    public float walk_speed;
    public float run_speed;
    public float detect_range;
    public int agent_priority;
    public float agent_radius;
    public byte endurance;
    public float stun_escape;
    public float respawn_time;
    public float exp;
    public float attack_speed;
}

public struct MonsterSpawnInfo
{
    public float hp;
    public Vector3 spawnPos;
    public Vector3 spawnRot;
}

public static class MonsterInfoTableManager
{
    public static List<MonsterInfo> mobInfoList = new List<MonsterInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            MonsterInfo mobInfo;

            mobInfo.id = ushort.Parse(datas[0]);
            mobInfo.prefab_name = datas[1];
            mobInfo.monster_name = datas[2];
            mobInfo.monster_type = byte.Parse(datas[3]);
            mobInfo.hp = float.Parse(datas[4]);
            mobInfo.attack_damage = float.Parse(datas[5]);
            mobInfo.attack_distance = float.Parse(datas[6]);
            mobInfo.attack_angle = float.Parse(datas[7]);
            mobInfo.skill_1_damage = float.Parse(datas[8]);
            mobInfo.skill_1_distance = float.Parse(datas[9]);
            mobInfo.skill_1_angle = float.Parse(datas[10]);
            mobInfo.skill_2_damage = float.Parse(datas[11]);
            mobInfo.skill_2_distance = float.Parse(datas[12]);
            mobInfo.skill_2_angle = float.Parse(datas[13]);
            mobInfo.skill_3_damage = float.Parse(datas[14]);
            mobInfo.skill_3_distance = float.Parse(datas[15]);
            mobInfo.skill_3_angle = float.Parse(datas[16]);
            mobInfo.walk_speed = float.Parse(datas[17]);
            mobInfo.run_speed = float.Parse(datas[18]);
            mobInfo.detect_range = float.Parse(datas[19]);
            mobInfo.agent_priority = int.Parse(datas[20]);
            mobInfo.agent_radius = float.Parse(datas[21]);
            mobInfo.endurance = byte.Parse(datas[22]);
            mobInfo.stun_escape = float.Parse(datas[23]);
            mobInfo.respawn_time = float.Parse(datas[24]);
            mobInfo.exp = float.Parse(datas[25]);
            mobInfo.attack_speed = float.Parse(datas[26]);

            mobInfoList.Add(mobInfo);
        }
    }

    public static string GetMonsterNameFromID(ushort mobID)
    {
        foreach (MonsterInfo mobinfo in mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.monster_name;
        }

        throw new System.NotSupportedException(mobID + " 에 해당하는 몬스터는 없습니다.");
    }

    public static string GetMonsterPrefabNameFromID(ushort mobID)
    {
        foreach (MonsterInfo mobinfo in mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.prefab_name;
        }

        throw new System.NotSupportedException(mobID + " 에 해당하는 몬스터는 없습니다.");
    }

    public static byte GetMonsterTypeFromMobID(ushort mobID)
    {
        foreach (MonsterInfo mobinfo in mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.monster_type;
        }

        throw new System.NotSupportedException(mobID + " 에 해당하는 몬스터는 없습니다.");
    }

    public static float GetMonsterOriginHpFromID(ushort mobID)
    {
        foreach (MonsterInfo mobinfo in mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.hp;
        }

        throw new System.NotSupportedException(mobID + " 에 해당하는 몬스터는 없습니다.");
    }
}
