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
    public byte endurance;
    public ushort id;
    public ushort index;
    public int thinking_param;
    public float hp;
    public float run_speed;
    public string monster_name;
    
    protected float detect_range;
    protected float skill_1_damage;
    protected float skill_1_distance;
    protected float skill_1_angle;
    protected float skill_2_damage;
    protected float skill_2_distance;
    protected float skill_2_angle;
    protected float skill_3_damage;
    protected float skill_3_distance;
    protected float skill_3_angle;
    protected float stun_escape;
    //

    public NavMeshAgent agent;
    public bool isMonsterAttackDone;
    public bool isStuned;
    public bool isRespawning;
    public byte endurance_stack;
    public SpawnInfo spawnInfo;

    [SerializeField]
    protected float currentStunTimer;
    protected float currentAttackDamage;
    protected float currentAttackDistance;
    protected float currentAttackAngle;
    
    protected const float THINKING_DURATION = 0.1f;
    protected const float STUN_DURATION = 1f;
    protected const float ANGULAR_SPEED = 999f;
    protected const float MIN_SIGHT_ANGLE = 20f;
    protected const float DISABLE_TIME = 5f;
    protected const float RESPAWN_TIME = 5f;

    private const float COLOR_LERF_SPEED = 0.1f;

    private void Start()
    {
        Setup();

        StartCoroutine(Thinking(THINKING_DURATION));

        StartCoroutine(StunCooldown(STUN_DURATION));
    }

    private void Update()
    {
        Detector();
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

    public virtual void Dead()
    {
        foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in QuestManager.instance.playerQuests)
        {
            if (quests.Key.target_monster_id == this.id)
                QuestManager.instance.playerQuests[quests.Key].target_monster_hunted++;
        }

        agent.enabled = false;
        Invoke("Disabled", DISABLE_TIME);
    }

    protected void Disabled()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", RESPAWN_TIME);
    }

    protected void Respawn()
    {
        gameObject.SetActive(true);
        agent.enabled = true;
        this.hp = spawnInfo.hp;
        this.transform.position = spawnInfo.spawnPos;
        this.transform.rotation = Quaternion.Euler(spawnInfo.spawnRot);
        this.thinking_param = (int)MonsterAnimation.AniType.IDLE;

        // Respawn Alpha Blending
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        StandardShaderUtils.ChangeRenderMode(smr.material, StandardShaderUtils.BlendMode.Fade);

        smr.material.color = new Color(1, 1, 1, 0);

        StartCoroutine(Thinking(THINKING_DURATION));
        StartCoroutine(RespawnEffect(smr.material));
    }

    protected virtual IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            if (!isRespawning)
            {
                switch (thinking_param)
                {
                    case (int)MonsterAnimation.AniType.IDLE:
                        if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0 && !isStuned)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.WALK, (int)MonsterAnimation.AniType.RUN + 1);

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0 &&
                            !isStuned)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.WALK:
                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.RUN:
                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.ATTACK:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;

                }
            }
        }
    }

    protected IEnumerator StunCooldown(float stunDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(stunDuration);

        while (true)
        {
            yield return null;

            if (isStuned)
            {
                yield return wt;
                currentStunTimer++;

                if (currentStunTimer == stun_escape)
                {
                    isStuned = false;
                    currentStunTimer = 0;
                }
            }
        }
    }

    protected virtual void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)MonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle && !GameManager.instance.controller.isPlayerWantToRoll)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
    }

    protected void Setup()
    {
        SpawnInfoSetup();
        InitallizeMobInfoFromTable();
    }

    private void SpawnInfoSetup()
    {
        this.spawnInfo.hp = MonsterInfoTableManager.GetMonsterOriginHpFromID(id);
        this.spawnInfo.spawnPos = this.transform.position;
        this.spawnInfo.spawnRot = this.transform.rotation.eulerAngles;
    }

    private void InitallizeMobInfoFromTable()
    {
        foreach (MonsterInfo mobinfo in MonsterInfoTableManager.mobInfoList)
        {
            if (mobinfo.id == id)
            {
                hp = mobinfo.hp;
                monster_name = mobinfo.monster_name;
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
                endurance = mobinfo.endurance;
                skill_1_damage = mobinfo.skill_1_damage;
                skill_1_distance = mobinfo.skill_1_distance;
                skill_1_angle = mobinfo.skill_1_angle;
                skill_2_damage = mobinfo.skill_2_damage;
                skill_2_distance = mobinfo.skill_2_distance;
                skill_2_angle = mobinfo.skill_2_angle;
                skill_3_damage = mobinfo.skill_3_damage;
                skill_3_distance = mobinfo.skill_3_distance;
                skill_3_angle = mobinfo.skill_3_angle;
                stun_escape = mobinfo.stun_escape;
                return;
            }
        }
    }

    private IEnumerator RespawnEffect(Material mat)
    {
        this.isRespawning = true;
        float time = 0f;
        Color finish = new Color(1, 1, 1, 1);
        

        while (mat.color.a < 0.9f)
        {
            yield return null;

            time += Time.deltaTime;
            mat.color = Color.Lerp(mat.color, finish, time * COLOR_LERF_SPEED);
        }

        mat.color = finish;
        StandardShaderUtils.ChangeRenderMode(mat, StandardShaderUtils.BlendMode.Opaque);
        this.isRespawning = false;
    }
}

public class CommonMonster : Monster
{
    private void Start()
    {
        Setup();

        StartCoroutine(Thinking(THINKING_DURATION));

        StartCoroutine(StunCooldown(STUN_DURATION));
    }

    private void Update()
    {
        Detector();
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)MonsterAnimation.AniType.ATTACK:
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
    private void Start()
    {
        Setup();

        StartCoroutine(Thinking(THINKING_DURATION));
    }

    private void Update()
    {
        Detector();
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            if (!isRespawning)
            {
                switch (thinking_param)
                {
                    case (int)MonsterAnimation.AniType.IDLE:
                        if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.WALK, (int)MonsterAnimation.AniType.RUN + 1);

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.ATTACK, (int)MonsterAnimation.AniType.SKILL_01 + 1);

                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_01;
                        break;

                    case (int)MonsterAnimation.AniType.WALK:
                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_01;

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.RUN:
                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_01;

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.ATTACK:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;

                    case (int)MonsterAnimation.AniType.SKILL_01:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;
                }
            }
        }
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)MonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;

            case (int)MonsterAnimation.AniType.SKILL_01:
                currentAttackDamage = skill_1_damage;
                currentAttackDistance = skill_1_distance;
                currentAttackAngle = skill_1_angle;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle && !GameManager.instance.controller.isPlayerWantToRoll)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
    }
}

public class BossMonster : Monster
{
    private void Start()
    {
        Setup();

        StartCoroutine(Thinking(THINKING_DURATION));
    }

    private void Update()
    {
        Detector();
    }

    protected override IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            if (!isRespawning)
            {
                switch (thinking_param)
                {
                    case (int)MonsterAnimation.AniType.IDLE:
                        if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.WALK, (int)MonsterAnimation.AniType.RUN + 1);

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.ATTACK, (int)MonsterAnimation.AniType.SKILL_03 + 1);

                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.SKILL_01, (int)MonsterAnimation.AniType.SKILL_03 + 1);

                        if (currentDistanceWithPlayer > skill_1_distance && currentDistanceWithPlayer <= skill_2_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.SKILL_02, (int)MonsterAnimation.AniType.SKILL_03 + 1);

                        if (currentDistanceWithPlayer > skill_2_distance && currentDistanceWithPlayer <= skill_3_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_03 + 1;
                        break;

                    case (int)MonsterAnimation.AniType.WALK:
                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_01;

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.RUN:
                        if (currentDistanceWithPlayer > attack_distance && currentDistanceWithPlayer <= skill_1_distance &&
                            currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.SKILL_01;

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE &&
                            GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = (int)MonsterAnimation.AniType.ATTACK;
                        break;

                    case (int)MonsterAnimation.AniType.ATTACK:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;

                    case (int)MonsterAnimation.AniType.SKILL_01:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;

                    case (int)MonsterAnimation.AniType.SKILL_02:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;

                    case (int)MonsterAnimation.AniType.SKILL_03:
                        if (isMonsterAttackDone)
                            thinking_param = (int)MonsterAnimation.AniType.IDLE;
                        break;
                }
            }
        }
    }

    protected override void Attack(int ani_id)
    {
        switch (ani_id)
        {
            case (int)MonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;

            case (int)MonsterAnimation.AniType.SKILL_01:
                currentAttackDamage = skill_1_damage;
                currentAttackDistance = skill_1_distance;
                currentAttackAngle = skill_1_angle;
                break;

            case (int)MonsterAnimation.AniType.SKILL_02:
                currentAttackDamage = skill_2_damage;
                currentAttackDistance = skill_2_distance;
                currentAttackAngle = skill_2_angle;
                break;

            case (int)MonsterAnimation.AniType.SKILL_03:
                currentAttackDamage = skill_3_damage;
                currentAttackDistance = skill_3_distance;
                currentAttackAngle = skill_3_angle;
                break;
        }

        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle && !GameManager.instance.controller.isPlayerWantToRoll)
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
    }
}
