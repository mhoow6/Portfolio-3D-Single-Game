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
    public ushort index;
    public int thinking_param;
    public float hp;
    public float run_speed;
    public string monster_name;
    public float exp;
    
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
    public float stun_escape;
    protected float respawn_time;
    public float attack_speed;
    //

    public NavMeshAgent agent;
    public bool isStuned;
    public bool isAlphaBlending;
    public bool isMonsterAttackDone;
    public byte endurance_stack;
    public MonsterSpawnInfo spawnInfo;
    public Transform head;

    [SerializeField]
    protected float currentStunTimer;
    [SerializeField]
    protected float currentAttackCooldown;
    protected float currentAttackDamage;
    protected float currentAttackDistance;
    protected float currentAttackAngle;
    protected SkinnedMeshRenderer smr;
    protected IEnumerator thinking;

    protected const float THINKING_DURATION = 0.1f;
    protected const float MIN_SIGHT_ANGLE = 20f;

    private const float ANGULAR_SPEED = 999f;
    private const float DISABLE_TIME = 5f;
    private const float COLOR_LERF_SPEED = 0.1f;

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
        thinking = Thinking(THINKING_DURATION);
        GetNodeObject(this.transform, "Head", ref head);

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
                attack_speed = mobinfo.attack_speed;
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
        else if (MonsterInfoTableManager.GetMonsterTypeFromMobID(mobID) == (byte)mobType.BOSS_MONSTER)
        {
            monster = obj.AddComponent<BossMonster>();
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

        // Level up
        GameManager.instance.controller.player.LevelUpCheck();


        agent.enabled = false;
        

        Invoke("DeadState", DISABLE_TIME);
    }

    public IEnumerator StunCooldown(float duration)
    {
        stunEffect = EffectManager.instance.CreateStarStunEffect(id, head);
        stunEffect.ps.Play();

        StopCoroutine(thinking);

        WaitForSeconds wt = new WaitForSeconds(1f);
        
        while (currentStunTimer != duration)
        {
            currentStunTimer++;
            yield return wt;
        }

        StartCoroutine(thinking);

        isStuned = false;
        stunEffect.ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        stunEffect = null;
        currentStunTimer = 0;
    }

    public IEnumerator AttackCooldown(float duration)
    {
        StopCoroutine(thinking);

        WaitForSeconds wt = new WaitForSeconds(1f);

        while (currentAttackCooldown != duration)
        {
            currentAttackCooldown++;
            yield return wt;
        }

        if (!isStuned)
            StartCoroutine(thinking);

        currentAttackCooldown = 0;
    }

    protected virtual IEnumerator Thinking(float thinkingDuration)
    {
        WaitForSeconds wt = new WaitForSeconds(thinkingDuration);

        while (true)
        {
            yield return wt;

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

    private void GetNodeObject(Transform parent, string nodeName, ref Transform node)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            // 이미 nodeName에 맞는 것을 찾아서 null이 아닐 경우 의미없는 호출을 방지하기 위해 함수 종료
            if (node != null)
                return;

            Transform child = parent.GetChild(i);

            if (child.name != nodeName)
            {
                // 자식이 또다른 자식을 가질 경우 자식의 자식들을 탐색
                if (child.childCount != 0)
                    GetNodeObject(child, nodeName, ref node);
            }

            if (child.name == nodeName)
                node = child;
        }
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
        {
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
            StartCoroutine(AttackCooldown(attack_speed));
        }    
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
        {
            GameManager.instance.controller.player.currentHp -= currentAttackDamage;
            StartCoroutine(AttackCooldown(attack_speed));
        }
            
    }
}
