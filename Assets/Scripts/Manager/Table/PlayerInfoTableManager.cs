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
    public float skill_01_damage;
    public float skill_01_distance;
    public float skill_02_angle;
    public float skill_02_mp;
    public float skill_02_sp;
    public float skill_02_cooldown;
    public float skill_02_damage;
    public float skill_02_distance;
    public float walk_speed;
    public float run_speed;
    public float run_sp;
    public float combat_walk_speed;
    public float roll_distance;
    public float roll_sp;
    public ushort basic_weapon_id;
    public float sp_recovery_point;
    public float running_sp_reduction_rate;
    public float exp;
}

public struct PlayerTempInfo
{
    public byte level;
    public float currentHp;
    public float currentMp;
    public float currentSp;
    public float currentExp;
}

public static class PlayerInfoTableManager
{
    public static PlayerInfo playerInfo;
    public static PlayerTempInfo playerTempInfo;

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

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
            playerInfo.skill_01_damage = float.Parse(datas[15]);
            playerInfo.skill_01_distance = float.Parse(datas[16]);
            playerInfo.skill_02_angle = float.Parse(datas[17]);
            playerInfo.skill_02_mp = float.Parse(datas[18]);
            playerInfo.skill_02_sp = float.Parse(datas[19]);
            playerInfo.skill_02_cooldown = float.Parse(datas[20]);
            playerInfo.skill_02_damage = float.Parse(datas[21]);
            playerInfo.skill_02_distance = float.Parse(datas[22]);
            playerInfo.walk_speed = float.Parse(datas[23]);
            playerInfo.run_speed = float.Parse(datas[24]);
            playerInfo.run_sp = float.Parse(datas[25]);
            playerInfo.combat_walk_speed = float.Parse(datas[26]);
            playerInfo.roll_distance = float.Parse(datas[27]);
            playerInfo.roll_sp = float.Parse(datas[28]);
            playerInfo.basic_weapon_id = ushort.Parse(datas[29]);
            playerInfo.sp_recovery_point = float.Parse(datas[30]);
            playerInfo.running_sp_reduction_rate = float.Parse(datas[31]);
            playerInfo.exp = float.Parse(datas[32]);
        }
    }

    public static void LoadTempTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTempTable(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            playerTempInfo.level = byte.Parse(datas[0]);
            playerTempInfo.currentHp = float.Parse(datas[1]);
            playerTempInfo.currentMp = float.Parse(datas[2]);
            playerTempInfo.currentSp = float.Parse(datas[3]);
            playerTempInfo.currentExp = float.Parse(datas[4]);
        }
    }
}
