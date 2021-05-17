using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct MonsterInfo
{
    public ushort id;
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
}

public static class MonsterInfoTableManager
{
    public static List<MonsterInfo> mobInfoList = new List<MonsterInfo>();

    public static void LoadTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            sr.ReadLine(); // ù ���ڵ� ����

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                MonsterInfo mobInfo;

                mobInfo.id = ushort.Parse(datas[0]);
                mobInfo.monster_name = datas[1];
                mobInfo.monster_type = byte.Parse(datas[2]);
                mobInfo.hp = float.Parse(datas[3]);
                mobInfo.attack_damage = float.Parse(datas[4]);
                mobInfo.attack_distance = float.Parse(datas[5]);
                mobInfo.attack_angle = float.Parse(datas[6]);
                mobInfo.skill_1_damage = float.Parse(datas[7]);
                mobInfo.skill_1_distance = float.Parse(datas[8]);
                mobInfo.skill_1_angle = float.Parse(datas[9]);
                mobInfo.skill_2_damage = float.Parse(datas[10]);
                mobInfo.skill_2_distance = float.Parse(datas[11]);
                mobInfo.skill_2_angle = float.Parse(datas[12]);
                mobInfo.skill_3_damage = float.Parse(datas[13]);
                mobInfo.skill_3_distance = float.Parse(datas[14]);
                mobInfo.skill_3_angle = float.Parse(datas[15]);
                mobInfo.walk_speed = float.Parse(datas[16]);
                mobInfo.run_speed = float.Parse(datas[16]);

                mobInfoList.Add(mobInfo);
            }

            sr.Close();
        }
    }

    public static string GetMonsterNameFromID(ushort mobID)
    {
        foreach (MonsterInfo mobinfo in mobInfoList)
        {
            if (mobID == mobinfo.id)
                return mobinfo.monster_name;
        }

        throw new System.NotSupportedException(mobID + " �� �ش��ϴ� ���ʹ� �����ϴ�.");
    }

    private static ushort GetMonsterIDFromName(string mobName)
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobName == mobinfo.monster_name)
                return mobinfo.id;
        }

        throw new System.NotSupportedException("�����߿�" + mobName + " �� �����ϴ�.");
    }
}
