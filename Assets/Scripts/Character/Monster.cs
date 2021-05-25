using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum mobType
{
    COMMON_MONSTER = 1,
    ELITE_MONSTER,
    BOSS_MONSTER
}

public class Monster : Character
{
    public byte monster_type;
    public ushort id;
    public ushort index;
    public int thinking_param;
    public float hp;
    public float run_speed;
    public float detect_range;
    protected const float THINKING_DURATION = 1f;
    protected const float ANGULAR_SPEED = 999f;

    public NavMeshAgent agent;

    private void Awake()
    {
        hp = 100; // TestZone Only
    }
    public virtual void Dead()
    {
        if (hp < 0)
            hp = 0;

        Invoke("Disabled", 5f);
    }

    public static Monster AddMonsterComponent(GameObject obj, ushort mobID)
    {
        Monster monster;

        if (MonsterInfoTableManager.GetMonsterTypeFromMobID(mobID) == (byte)mobType.COMMON_MONSTER)
        {
            monster = obj.AddComponent<CommonMonster>();
            return monster;
        }
        else if (MonsterInfoTableManager.GetMonsterTypeFromMobID(mobID) == (byte)mobType.ELITE_MONSTER)
        {
            monster = obj.AddComponent<EliteMonster>();
            return monster;
        }
        else if (MonsterInfoTableManager.GetMonsterTypeFromMobID(mobID) == (byte)mobType.BOSS_MONSTER)
        {
            monster = obj.AddComponent<BossMonster>();
            return monster;
        }

        throw new System.NotSupportedException(mobID.ToString() + " 에 해당하는 몬스터가 없어 스크립트 추가에 실패했습니다.");
    }

    // abstract?
    protected virtual void InitallizeMobInfoFromTable()
    {

    }
    protected virtual IEnumerator Thinking(float thinkingDuration)
    {
        yield return null;
    }
    protected virtual void Move()
    {
    }

    protected void Disabled()
    {
        gameObject.SetActive(false);
    }
}

public class CommonMonster : Monster
{
    public float currentDistanceWithPlayer;

    private void Start()
    {
        InitallizeMobInfoFromTable();

        StartCoroutine(Thinking(THINKING_DURATION));
    }


    protected override void Move()
    {
        if (thinking_param == (int)MonsterAnimation.BasicAniType.RUN)
        {
            agent.speed = run_speed;
            agent.acceleration = run_speed;
        }

        agent.destination = GameManager.instance.controller.player.transform.position;
    }

    protected override void InitallizeMobInfoFromTable()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == id)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                agent.speed = mobinfo.walk_speed;
                agent.acceleration = mobinfo.walk_speed;
                agent.angularSpeed = ANGULAR_SPEED;
                run_speed = mobinfo.run_speed;
                detect_range = mobinfo.detect_range;
                return;
            }
        }
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            switch(thinking_param)
            {
                case (int)MonsterAnimation.BasicAniType.IDLE:
                    currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);

                    if (currentDistanceWithPlayer <= detect_range)
                        thinking_param = Random.Range((int)MonsterAnimation.BasicAniType.WALK, (int)MonsterAnimation.BasicAniType.RUN);
                    break;

                case (int)MonsterAnimation.BasicAniType.WALK:
                    currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);

                    if (currentDistanceWithPlayer <= attack_distance)
                        thinking_param = (int)MonsterAnimation.BasicAniType.ATTACK;
                    break;

                case (int)MonsterAnimation.BasicAniType.RUN:
                    currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);

                    if (currentDistanceWithPlayer <= attack_distance)
                        thinking_param = (int)MonsterAnimation.BasicAniType.ATTACK;
                    break;

                case (int)MonsterAnimation.BasicAniType.ATTACK:
                    if (GameManager.instance.controller.player.currentHp <= 0)
                        thinking_param = (int)MonsterAnimation.BasicAniType.IDLE;
                    break;

            }
        }
    }
}

public class EliteMonster : Monster
{
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;

    private void Start()
    {
        InitallizeMobInfoFromTable();
    }

    protected override void InitallizeMobInfoFromTable()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == id)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;
                detect_range = mobinfo.detect_range;
                return;
            }
        }
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        throw new System.NotImplementedException();
    }
}

public class BossMonster : Monster
{
    public float skill_1_damage;
    public float skill_1_distance;
    public float skill_1_angle;
    public float skill_2_damage;
    public float skill_2_distance;
    public float skill_2_angle;
    public float skill_3_damage;
    public float skill_3_distance;
    public float skill_3_angle;

    private void Start()
    {
        InitallizeMobInfoFromTable();
    }

    protected override void InitallizeMobInfoFromTable()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == id)
            {
                hp = mobinfo.hp;
                attack_damage = mobinfo.attack_damage;
                attack_distance = mobinfo.attack_distance;
                attack_angle = mobinfo.attack_angle;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;
                skill_2_damage = mobinfo.skill_2_damage;
                skill_2_distance = mobinfo.skill_2_distance;
                skill_2_angle = mobinfo.skill_2_angle;
                skill_3_damage = mobinfo.skill_3_damage;
                skill_3_distance = mobinfo.skill_3_distance;
                skill_3_angle = mobinfo.skill_3_angle;
                monster_type = mobinfo.monster_type;
                walk_speed = mobinfo.walk_speed;
                run_speed = mobinfo.run_speed;
                detect_range = mobinfo.detect_range;
                return;
            }
        }
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        throw new System.NotImplementedException();
    }
}
