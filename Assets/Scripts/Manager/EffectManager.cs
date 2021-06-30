using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public List<Effect> effects = new List<Effect>();

    private Vector3 POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_POS = new Vector3(-0.082f, 0.238f, 0.004f);
    private Vector3 POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_ROT = new Vector3(0, 0, 24.618f);
    private Vector3 POLYGONAL_METALON_SPIDER_STUN_EFFECT_POS = new Vector3(-0.358f, -0.75f, 0);
    private Vector3 POLYGONAL_METALON_SPIDER_STUN_EFFECT_ROT = Vector3.zero;
    private Vector3 FX_NORMAL_COMBAT_ATTACK_01_POS = new Vector3(0.3039f, 1.007f, 0.9700f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_01_ROT = new Vector3(-34.76f, 38.518f, 35.615f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_02_POS = new Vector3(0.3039f, 1.007f, 0.9700f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_02_ROT = new Vector3(-13.6f, 48.598f, 11.714f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_03_POS = new Vector3(0.6199f, 1.02f, 0.9700f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_03_ROT = new Vector3(17.899f, 47.508f, -15.723f);

    private Vector3 FX_SKILL_E_POS = new Vector3(0.3039f, 1.698f, 0.9700f);
    private Vector3 FX_SKILL_E_ROT = new Vector3(-34.76f, 38.518f, 35.615f);
    private Vector3 FX_SKILL_E_RIPPLE_ROT = new Vector3(0, 50f, 0);

    private const string attackEffectName = "FX_NormalAttack";
    public ushort attackEffectID;
    private const string starStunEffectName = "FX_StarStunned_01";
    public ushort starStunEffectID;
    private const string hitEffectName = "FX_NormalHit";
    public ushort hitEffectID;
    private const string eSkillAttackEffectName = "FX_ESkill_Attack";
    public ushort eSkillAttackEffectID;
    private const string eSkillBackEffectName = "FX_ESkill_Background";
    public ushort eSkillBackEffectID;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        attackEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(attackEffectName);
        starStunEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(starStunEffectName);
        hitEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(hitEffectName);
        eSkillAttackEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(eSkillAttackEffectName);
        eSkillBackEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(eSkillBackEffectName);
    }

    public Effect CreateStarStunEffect(ushort objID, Transform head)
    {
        // 1. 이펙트 풀에 이미 해당 위치에 비활성화 된 이펙트가 있는지 검사
        Effect existEffect = effects.Find(eff => eff.effectNode == head && eff.effectID == starStunEffectID && eff.ps.isStopped);

        if (existEffect != null)
            return existEffect;

        // 2. 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + starStunEffectName);
        Effect effect = Instantiate(_effect.AddComponent<Effect>());

        // 3. 데이터 추가
        effect.effectID = starStunEffectID;
        effect.effectNode = head;

        // 4. 새로 생긴 이펙트를 이펙트 풀에 추가 및 부모 지정
        effects.Add(effect);
        effect.transform.SetParent(head);

        // 5. 해당 몬스터에 맞게 이펙트 위치 조정
        if (objID >= 1003 && objID <= 1006) // PolyTope Studio에서 만든 몬스터 기준
        {
            effect.transform.localPosition = POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_POS;
            effect.transform.localRotation = Quaternion.Euler(POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_ROT);
        }

        if (objID >= 1000 && objID < 1002) // Polygonal Metalon에서 만든 거미 몬스터
        {
            effect.transform.localPosition = POLYGONAL_METALON_SPIDER_STUN_EFFECT_POS;
            effect.transform.localRotation = Quaternion.Euler(POLYGONAL_METALON_SPIDER_STUN_EFFECT_ROT);
        }

        // 6. 해당 이펙트 return
        return effect;
    }

    public PlayerAttackEffect CreateAttackEffect(int aniIndex)
    {
        // 이펙트 풀에 이미 해당 노드에 비활성화 된 이펙트가 있는지 검사
        Effect _existEffect = effects.Find(eff => eff.effectID == attackEffectID && eff.ps.isStopped);
        PlayerAttackEffect existEffect = (PlayerAttackEffect)_existEffect;

        if (existEffect != null)
        {
            // 해당 공격에 맞게 이펙트 조정 및 데이터 추가
            AttackEffectSetup(existEffect, aniIndex);
            return existEffect;
        }

        // 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + attackEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerAttackEffect eff = effect.AddComponent<PlayerAttackEffect>();
        
        // 새로 생긴 이펙트를 이펙트 풀에 추가 및 부모 지정
        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        // 해당 공격에 맞게 이펙트 조정 및 데이터 추가
        AttackEffectSetup(eff, aniIndex);

        // 해당 이펙트 return
        return eff;
    }

    public PlayerAttackHitEffect CreateHitEffect(Monster hitMob)
    {
        // 재사용 가능 이펙트 검사
        Effect _existEffect = effects.Find(eff => eff.effectID == hitEffectID && !eff.gameObject.activeSelf);
        PlayerAttackHitEffect existEffect = (PlayerAttackHitEffect)_existEffect;

        if (existEffect != null)
        {
            existEffect.gameObject.SetActive(true);
            existEffect.hitLight.gameObject.SetActive(true);
            existEffect.transform.SetParent(this.transform);
            existEffect.transform.position = SetHitEffectPosition(hitMob);
            return existEffect;
        }

        // 새 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + hitEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerAttackHitEffect eff = effect.AddComponent<PlayerAttackHitEffect>();

        // 이펙트 풀에 추가
        effects.Add(eff);

        // 위치 Setup
        effect.transform.SetParent(this.transform);
        effect.transform.position = SetHitEffectPosition(hitMob);

        // 해당 이펙트 return
        return eff;
    }

    public PlayerESkillAttackEffect CreateESkillAttackEffect()
    {
        Effect _existEffect = effects.Find(eff => eff.effectID == eSkillAttackEffectID && eff.ps.isStopped);
        PlayerESkillAttackEffect existEffect = (PlayerESkillAttackEffect)_existEffect;

        if (existEffect != null)
        {
            SkillAttackEffectSetup(existEffect);
            return existEffect;
        }

        GameObject _effect = Resources.Load<GameObject>("Particle/" + eSkillAttackEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerESkillAttackEffect eff = effect.AddComponent<PlayerESkillAttackEffect>();

        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        SkillAttackEffectSetup(eff);

        return eff;
    }

    public PlayerESkillBackEffect CreateESkillBackEffect()
    {
        Effect _existEffect = effects.Find(eff => eff.effectID == eSkillBackEffectID && !eff.gameObject.activeSelf);
        PlayerESkillBackEffect existEffect = (PlayerESkillBackEffect)_existEffect;

        if (existEffect != null)
        {
            existEffect.gameObject.SetActive(true);
            existEffect.transform.position = GameManager.instance.controller.player.transform.position;
            existEffect.transform.rotation = GameManager.instance.controller.player.transform.rotation;
            existEffect.rippleEffect.transform.localRotation = Quaternion.Euler(FX_SKILL_E_RIPPLE_ROT);
            return existEffect;
        }
            
        GameObject _effect = Resources.Load<GameObject>("Particle/" + eSkillBackEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerESkillBackEffect eff = effect.AddComponent<PlayerESkillBackEffect>();

        eff.effectID = eSkillBackEffectID;
        eff.effectNode = this.transform;
        eff.transform.position = GameManager.instance.controller.player.transform.position;
        eff.transform.rotation = GameManager.instance.controller.player.transform.rotation;

        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        eff.rippleEffect.transform.localRotation = Quaternion.Euler(FX_SKILL_E_RIPPLE_ROT);

        return eff;
    }

    private void AttackEffectSetup(Effect eff, int aniIndex)
    {
        eff.effectID = attackEffectID;
        eff.effectNode = GameManager.instance.controller.player.transform;

        switch (aniIndex)
        {
            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
                eff.transform.localPosition = FX_NORMAL_COMBAT_ATTACK_01_POS;
                eff.transform.localRotation = Quaternion.Euler(FX_NORMAL_COMBAT_ATTACK_01_ROT);
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_02:
                eff.transform.localPosition = FX_NORMAL_COMBAT_ATTACK_02_POS;
                eff.transform.localRotation = Quaternion.Euler(FX_NORMAL_COMBAT_ATTACK_02_ROT);
                break;

            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_03:
                eff.transform.localPosition = FX_NORMAL_COMBAT_ATTACK_03_POS;
                eff.transform.localRotation = Quaternion.Euler(FX_NORMAL_COMBAT_ATTACK_03_ROT);
                break;
        }
    }

    private void SkillAttackEffectSetup(Effect eff)
    {
        eff.effectID = eSkillAttackEffectID;
        eff.effectNode = GameManager.instance.controller.player.transform;

        eff.transform.localPosition = FX_SKILL_E_POS;
        eff.transform.localRotation = Quaternion.Euler(FX_SKILL_E_ROT);
    }

    private Vector3 SetHitEffectPosition(Monster hitMob)
    {
        float minDistance = Vector3.Distance(GameManager.instance.controller.player.transform.position, hitMob.hitEffectsPos[0].position);
        Vector3 effectPos = hitMob.hitEffectsPos[0].position;
        float dis; // temp

        for (int i = 1; i < hitMob.hitEffectsPos.Length; i++)
        {
            dis = Vector3.Distance(GameManager.instance.controller.player.transform.position, hitMob.hitEffectsPos[i].position);

            if (dis < minDistance)
            {
                minDistance = dis;
                effectPos = hitMob.hitEffectsPos[i].position;
            }
        }

        return effectPos;
    }
}
