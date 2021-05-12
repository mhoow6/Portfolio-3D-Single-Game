using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct MonsterInfo
{
    public ushort id;
    public string monster_name;
    public float hp;
    public float attack_damage;
    public float attack_distance;
    public float attack_angle;
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
}

public static class MonsterInfoTableManager
{
    public static List<MonsterInfo> mobInfoList = new List<MonsterInfo>();

    public static void LoadTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            sr.ReadLine(); // 첫 레코드 제외

            while ((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                MonsterInfo mobInfo;

                mobInfo.id = ushort.Parse(datas[0]);
                mobInfo.monster_name = datas[1];
                mobInfo.hp = float.Parse(datas[2]);
                mobInfo.attack_damage = float.Parse(datas[3]);
                mobInfo.attack_distance = float.Parse(datas[4]);
                mobInfo.attack_angle = float.Parse(datas[5]);
                mobInfo.skill_1_damage = float.Parse(datas[6]);
                mobInfo.skill_1_distance = float.Parse(datas[7]);
                mobInfo.skill_1_angle = float.Parse(datas[8]);

                mobInfoList.Add(mobInfo);
            }

            sr.Close();
        }
    }
}
