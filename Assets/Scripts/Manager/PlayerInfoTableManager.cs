using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct PlayerInfo
{
    public byte level;
    public float hp;
    public float mp;
    public float sp;
    public float attack_01_angle;
    public float attack_02_angle;
    public float attack_sp;
    public float combat_attack_01_angle;
    public float combat_attack_02_angle;
    public float combat_attack_03_angle;
    public float combat_attack_sp;
    public float skill_01_angle;
    public float skill_01_mp;
    public float skill_01_sp;
    public float skill_01_cooldown;
    public float skill_02_angle;
    public float skill_02_mp;
    public float skill_02_sp;
    public float skill_02_cooldown;
    public float walk_speed;
    public float run_speed;
    public float combat_walk_speed;
    public float combat_run_speed;
    public float roll_distance;
    public ushort basic_weapon_id;
    public ushort equip_weapon_id;
}

public static class PlayerInfoTableManager
{
    public static PlayerInfo playerInfo;

    public static void LoadTable(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string line = string.Empty;

            sr.ReadLine();

            while((line = sr.ReadLine()) != null)
            {
                string[] datas = line.Split(',');

                playerInfo.level = byte.Parse(datas[0]);
                playerInfo.hp = float.Parse(datas[1]);
                playerInfo.mp = float.Parse(datas[2]);
                playerInfo.sp = float.Parse(datas[3]);
                playerInfo.attack_01_angle = float.Parse(datas[4]);
                playerInfo.attack_02_angle = float.Parse(datas[5]);
                playerInfo.attack_sp = float.Parse(datas[6]);
                playerInfo.combat_attack_01_angle = float.Parse(datas[7]);
                playerInfo.combat_attack_02_angle = float.Parse(datas[8]);
                playerInfo.combat_attack_03_angle = float.Parse(datas[9]);
                playerInfo.combat_attack_sp = float.Parse(datas[10]);
                playerInfo.skill_01_angle = float.Parse(datas[11]);
                playerInfo.skill_01_mp = float.Parse(datas[12]);
                playerInfo.skill_01_sp = float.Parse(datas[13]);
                playerInfo.skill_01_cooldown = float.Parse(datas[14]);
                playerInfo.skill_02_angle = float.Parse(datas[15]);
                playerInfo.skill_02_mp = float.Parse(datas[16]);
                playerInfo.skill_02_sp = float.Parse(datas[17]);
                playerInfo.skill_02_cooldown = float.Parse(datas[18]);
                playerInfo.walk_speed = float.Parse(datas[19]);
                playerInfo.run_speed = float.Parse(datas[20]);
                playerInfo.combat_walk_speed = float.Parse(datas[21]);
                playerInfo.combat_run_speed = float.Parse(datas[22]);
                playerInfo.roll_distance = float.Parse(datas[23]);
                playerInfo.basic_weapon_id = ushort.Parse(datas[24]);
                playerInfo.equip_weapon_id = ushort.Parse(datas[25]);
            }

            sr.Close();
        }
    }
}
