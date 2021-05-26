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
    // Load From Table
    public byte monster_type;
    public ushort id;
    public ushort index;
    public int thinking_param;
    public float hp;
    public float run_speed;
    public float detect_range;
    //

    public NavMeshAgent agent;

    [SerializeField]
    protected float currentDistanceWithPlayer;
    [SerializeField]
    protected float currentAngleWithPlayer;
    protected float currentAttackDamage;
    protected float currentAttackDistance;
    protected float currentAttackAngle;
    protected const float THINKING_DURATION = 1f;
    protected const float ANGULAR_SPEED = 999f;
    protected const float MIN_SIGHT_ANGLE = 20f;
    
        
    private void Start()
    {
        StartCoroutine(Thinking(THINKING_DURATION)); // Unit Test
    }

    private void Update()
    {
        // Unit Test
        currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);
        currentAngleWithPlayer = Mathf.Acos(Vector3.Dot
                (transform.forward,
                (GameManager.instance.controller.player.transform.position - transform.position).normalized)
                ) * Mathf.Rad2Deg;
    }

    // Unit Test
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

    protected void Disabled()
    {
        gameObject.SetActive(false);
    }

    protected virtual void InitallizeMobInfoFromTable()
    {

    }

    // Unit Test
    protected virtual IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;
            
            switch (thinking_param)
            {
                case (int)CommonMonsterAnimation.AniType.IDLE:
                    if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)CommonMonsterAnimation.AniType.WALK, (int)CommonMonsterAnimation.AniType.RUN);

                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.WALK:
                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.RUN:
                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.ATTACK:
                    if (GameManager.instance.controller.player.currentHp <= 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.IDLE;

                    if (currentDistanceWithPlayer > attack_distance || currentAngleWithPlayer > attack_angle && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)CommonMonsterAnimation.AniType.WALK, (int)CommonMonsterAnimation.AniType.RUN);
                    break;

            }
        }
    }

    // Unit Test
    protected virtual void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)CommonMonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
    }
}

public class CommonMonster : Monster
{
    private void Start()
    {
        InitallizeMobInfoFromTable();

        StartCoroutine(Thinking(THINKING_DURATION));
    }

    public override void Dead()
    {
        agent.enabled = false;
        Invoke("Disabled", 5f);
    }

    private void Update()
    {
        currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);
        currentAngleWithPlayer = Mathf.Acos(Vector3.Dot
                (transform.forward,
                (GameManager.instance.controller.player.transform.position - transform.position).normalized)
                ) * Mathf.Rad2Deg;
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            switch (thinking_param)
            {
                case (int)CommonMonsterAnimation.AniType.IDLE:
                    if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)CommonMonsterAnimation.AniType.WALK, (int)CommonMonsterAnimation.AniType.RUN + 1);

                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.WALK:
                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.RUN:
                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)CommonMonsterAnimation.AniType.ATTACK:
                    if (GameManager.instance.controller.player.currentHp <= 0)
                        thinking_param = (int)CommonMonsterAnimation.AniType.IDLE;

                    if (currentDistanceWithPlayer > attack_distance || currentAngleWithPlayer > attack_angle && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)CommonMonsterAnimation.AniType.WALK, (int)CommonMonsterAnimation.AniType.RUN + 1);
                    break;

            }
        }
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
                detect_range = mobinfo.detect_range;
                agent.speed = mobinfo.walk_speed;
                agent.acceleration = mobinfo.walk_speed;
                agent.angularSpeed = ANGULAR_SPEED;
                agent.avoidancePriority = mobinfo.agent_priority;
                agent.radius = mobinfo.agent_radius;
                return;
            }
        }
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)CommonMonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
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

        StartCoroutine(Thinking(THINKING_DURATION));
    }

    private void Update()
    {
        currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);
        currentAngleWithPlayer = Mathf.Acos(Vector3.Dot
                (transform.forward,
                (GameManager.instance.controller.player.transform.position - transform.position).normalized)
                ) * Mathf.Rad2Deg;
    }

    public override void Dead()
    {
        agent.enabled = false;
        Invoke("Disabled", 5f);
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
                agent.speed = mobinfo.walk_speed;
                agent.acceleration = mobinfo.walk_speed;
                agent.angularSpeed = ANGULAR_SPEED;
                agent.avoidancePriority = mobinfo.agent_priority;
                agent.radius = mobinfo.agent_radius;
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

            switch (thinking_param)
            {
                case (int)EliteMonsterAnimation.AniType.IDLE:
                    if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)EliteMonsterAnimation.AniType.WALK, (int)EliteMonsterAnimation.AniType.RUN + 1);

                    if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                        currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)EliteMonsterAnimation.AniType.SKILL_01;

                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && 
                        GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)EliteMonsterAnimation.AniType.ATTACK, (int)EliteMonsterAnimation.AniType.SKILL_01 + 1);
                    break;

                case (int)EliteMonsterAnimation.AniType.WALK:
                    if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                        currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)EliteMonsterAnimation.AniType.SKILL_01;

                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                        GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)EliteMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)EliteMonsterAnimation.AniType.RUN:
                    if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                        currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)EliteMonsterAnimation.AniType.SKILL_01;

                    if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && 
                        GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)EliteMonsterAnimation.AniType.ATTACK;
                    break;

                case (int)EliteMonsterAnimation.AniType.ATTACK:
                    if (GameManager.instance.controller.player.currentHp <= 0)
                        thinking_param = (int)EliteMonsterAnimation.AniType.IDLE;

                    if (currentDistanceWithPlayer > attack_distance || currentAngleWithPlayer > attack_angle && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)EliteMonsterAnimation.AniType.WALK, (int)EliteMonsterAnimation.AniType.RUN + 1);
                    break;

                case (int)EliteMonsterAnimation.AniType.SKILL_01:
                    if (GameManager.instance.controller.player.currentHp <= 0)
                        thinking_param = (int)EliteMonsterAnimation.AniType.IDLE;

                    if (currentDistanceWithPlayer > skill_1_distance || currentAngleWithPlayer > skill_1_angle && GameManager.instance.controller.player.currentHp > 0)
                        thinking_param = Random.Range((int)EliteMonsterAnimation.AniType.WALK, (int)EliteMonsterAnimation.AniType.RUN + 1);
                    break;
            }
        }
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)EliteMonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                break;

            case (int)EliteMonsterAnimation.AniType.SKILL_01:
                currentAttackDamage = skill_1_damage;
                currentAttackDistance = skill_1_distance;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
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
                agent.speed = mobinfo.walk_speed;
                agent.acceleration = mobinfo.walk_speed;
                agent.angularSpeed = ANGULAR_SPEED;
                agent.avoidancePriority = mobinfo.agent_priority;
                agent.radius = mobinfo.agent_radius;
                return;
            }
        }
    }
}
