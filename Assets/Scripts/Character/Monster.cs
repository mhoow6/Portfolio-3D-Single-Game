using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum hitEffectPos
{
    FRONT = 0,
    BACK = 1,
    LEFT = 2,
    RIGHT = 3
}

enum mobType
{
    COMMON_MONSTER = 1,
    ELITE_MONSTER,
    BOSS_MONSTER
}

public class Monster : Character
{
    // Load From Table
    public byte endurance;
    public ushort index;
    public int thinking_param;
    public float hp;
    public float run_speed;
    public float exp;
    public float stun_escape;
    public float attack_duration;
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
    protected float respawn_time;
    protected byte monster_type;
    //

    public bool isStuned;
    public bool isAlphaBlending;
    public bool isAttackCooldown;
    public byte endurance_stack;
    public MonsterSpawnInfo spawnInfo;
    public NavMeshAgent agent;
    public Transform head;
    public IEnumerator thinking;
    public Transform[] hitEffectsPos = new Transform[4];

    protected float currentStunTimer;
    public float currentAttackCooldown;
    protected float currentAttackDamage;
    protected float currentAttackDistance;
    protected float currentAttackAngle;

    public bool isImmortal;
    public SkinnedMeshRenderer smr;
    public Color originEmissionColor;

    protected const float THINKING_DURATION = 0.1f;
    protected const float MIN_SIGHT_ANGLE = 20f;
    private const float ANGULAR_SPEED = 999f;
    private const float DISABLE_TIME = 5f;
    private const float COLOR_LERF_SPEED = 0.1f;

    public Effect _stunEffect { get => stunEffect; }
    private Effect stunEffect;

    private void Start()
    {
        Setup();
        StartCoroutine(thinking);
    }

    private void Update()
    {
        Detector();
    }

    protected void Setup()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        originEmissionColor = smr.material.GetColor("_EmissionColor");
        thinking = Thinking(THINKING_DURATION);
        GetNodeObject(this.transform, "Head", ref head);

        GetNodeObject(this.transform, "Forward_Hit_Effect", ref hitEffectsPos[(int)hitEffectPos.FRONT]);
        GetNodeObject(this.transform, "Backward_Hit_Effect", ref hitEffectsPos[(int)hitEffectPos.BACK]);
        GetNodeObject(this.transform, "Leftward_Hit_Effect", ref hitEffectsPos[(int)hitEffectPos.LEFT]);
        GetNodeObject(this.transform, "Rightward_Hit_Effect", ref hitEffectsPos[(int)hitEffectPos.RIGHT]);

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
                respawn_time = mobinfo.respawn_time;
                exp = mobinfo.exp;
                attack_duration = mobinfo.attack_duration;
                return;
            }
        }
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
        else if (MonsterInfoTableManager.GetMonsterPrefabNameFromID(mobID) == "Character_BR_FortGolem_01")
        {
            monster = obj.AddComponent<Golem>();
            return monster;
        }
        else if (MonsterInfoTableManager.GetMonsterPrefabNameFromID(mobID) == "DragonSoulEaterRedHP")
        {
            monster = obj.AddComponent<Dragon>();
            return monster;
        }

        throw new System.NotSupportedException(mobID.ToString() + " 에 해당하는 몬스터가 없어 스크립트 추가에 실패했습니다.");
    }

    public void Dead()
    {
        foreach (KeyValuePair<QuestInfo, PlayerQuestStateInfo> quests in QuestManager.instance.playerQuests)
        {
            if (quests.Key.target_monster_id == this.id)
                QuestManager.instance.playerQuests[quests.Key].target_monster_hunted++;
        }

        if (GameManager.instance.controller.player.level != PlayerExpInfoTableManager.playerExpInfo.Length)
            GameManager.instance.controller.player.currentExp += exp;

        if (GameManager.instance.controller.player.level == PlayerExpInfoTableManager.playerExpInfo.Length)
            GameManager.instance.controller.player.currentExp = 0f;

        hp = 0f;

        // Level up
        GameManager.instance.controller.player.LevelUpCheck();

        agent.enabled = false;
        
        Invoke("DeadState", DISABLE_TIME);
    }

    public IEnumerator StunCooldown(float duration)
    {
        stunEffect = EffectManager.instance.CreateStarStunEffect(id, head);
        stunEffect.self.Play();

        while (currentStunTimer <= duration)
        {
            yield return null;
            isStuned = true;
            currentStunTimer += Time.deltaTime;
        }

        isStuned = false;
        StartCoroutine(thinking);

        stunEffect.self.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        currentStunTimer = 0;
    }

    public IEnumerator AttackCooldown(float duration)
    {
        while (currentAttackCooldown <= duration)
        {
            isAttackCooldown = true;
            yield return null;
            currentAttackCooldown += Time.deltaTime;
        }

        isAttackCooldown = false;
        currentAttackCooldown = 0;
    }

    protected virtual IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

            if (!isAttackCooldown && !isStuned)
            {
                switch (thinking_param)
                {
                    case (int)MonsterAnimation.AniType.IDLE:
                        if (currentDistanceWithPlayer <= detect_range && GameManager.instance.controller.player.currentHp > 0)
                            thinking_param = Random.Range((int)MonsterAnimation.AniType.WALK, (int)MonsterAnimation.AniType.RUN + 1);

                        if (currentDistanceWithPlayer <= attack_distance && currentAngleWithPlayer < MIN_SIGHT_ANGLE && GameManager.instance.controller.player.currentHp > 0)
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
                }
            } 
        }
    }

    protected void Attack(int ani_id)
    {
        AttackType(ani_id);
        RealAttack();
    }

    private void RealAttack()
    {
        if (currentDistanceWithPlayer < currentAttackDistance && currentAngleWithPlayer < currentAttackAngle && !GameManager.instance.controller.isPlayerWantToRoll)
        {
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
            GameManager.instance.controller.player.KnockBackEffect(currentAttackDamage, transform.forward);
            AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.ALL, AudioCondition.PLAYER_HIT), 1f);
        }
    }

    protected virtual void AttackType(int ani_id)
    {

    }

    private void DeadState()
    {
        StartCoroutine(Disappear());

        if (monster_type != (byte)mobType.BOSS_MONSTER)
            Invoke("Respawn", respawn_time);
    }

    private IEnumerator Disappear()
    {
        yield return StartCoroutine(Utility.AlphaBlending(smr.material, 0, COLOR_LERF_SPEED));
        yield return StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        this.gameObject.SetActive(false);
        yield return null;
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        agent.enabled = true;
        this.hp = spawnInfo.hp;
        this.transform.position = spawnInfo.spawnPos;
        this.transform.rotation = Quaternion.Euler(spawnInfo.spawnRot);
        this.thinking_param = (int)MonsterAnimation.AniType.IDLE;

        StartCoroutine(Respawning());
    }

    private IEnumerator Respawning()
    {
        yield return StartCoroutine(Utility.AlphaBlending(smr.material, 1, COLOR_LERF_SPEED));
        yield return StartCoroutine(Thinking(THINKING_DURATION));
    }
}

public class CommonMonster : Monster
{
    private void Start()
    {
        Setup();
        StartCoroutine(thinking);
    }

    private void Update()
    {
        Detector();
    }

    protected override void AttackType(int ani_id)
    {
        switch (ani_id)
        {
            case (int)MonsterAnimation.AniType.ATTACK:
                currentAttackDamage = attack_damage;
                currentAttackDistance = attack_distance;
                currentAttackAngle = attack_angle;
                break;
        }
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

            if (!isAttackCooldown && !isStuned)
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
                }
            }
        }
    }

    protected override void AttackType(int ani_id)
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
    }

}

public class Golem : Monster
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

            if (!isAttackCooldown && !isStuned)
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
                }
            }
        }
    }

    protected override void AttackType(int ani_id)
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
    }
}

