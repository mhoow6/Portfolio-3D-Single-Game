using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monster
{
    public float _currentDistanceWithPlayer { get => currentDistanceWithPlayer; }
    public float _currentAngleWithPlayer { get => currentAngleWithPlayer; }

    public float _detectRange { get => detect_range; }

    public float _attackRange { get => attack_distance; }
    public float _attackAngle { get => attack_angle; }

    public float _tailAttackRange { get => skill_1_distance; }

    public float _currentAttackCooldown { get => currentAttackCooldown; }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        Detector();
    }

    public bool IsTailAttackHitable()
    {
        return false;
    }
}
