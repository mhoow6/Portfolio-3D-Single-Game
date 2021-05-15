using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    private void Awake()
    {
        hp = 1;
        attack_damage = 0;
        attack_distance = 0;
        attack_angle = 0;
    }
}

