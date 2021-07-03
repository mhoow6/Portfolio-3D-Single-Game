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
    private Vector3 FX_SKILL_Q_POS = new Vector3(0.8f, 1.33f, 0.84f);
    private Vector3 FX_SKILL_Q_ROT = new Vector3(0f, 40.106f, 0f);
    private Vector3 FX_SKILL_Q_SCALE = new Vector3(1.3f, 1f, 1.3f);

    private const string attackEffectName = "FX_NormalAttack";
    private const string starStunEffectName = "FX_StarStunned_01";
    private const string hitEffectName = "FX_NormalHit";
    private const string eSkillAttackEffectName = "FX_ESkill_Attack";
    private const string eSkillBackEffectName = "FX_ESkill_Background";
    private const string qSkillAttackEffectName = "FX_QSkill_Attack";
    private const string qSkillHitEffectName = "FX_QSkill_Hit";
    private const string qSkillBackEffectName = "FX_QSkill_Background";
    private const string footStepEffectName = "FX_Cartoony_Footstep";
    private const string fireBallEffectName = "FX_Fireball_Shooting_Straight_Trail_01";

    private void Awake()
    {
        instance = this;
    }

    public Effect CreateStarStunEffect(ushort objID, Transform head)
    {
        // 1. 이펙트 풀에 이미 해당 위치에 비활성화 된 이펙트가 있는지 검사
        Effect existEffect = effects.Find(eff => eff.effectNode == head && eff.gameObject.name == starStunEffectName && eff.self.isStopped);

        if (existEffect != null)
            return existEffect;

        // 2. 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + starStunEffectName);
        Effect effect = Instantiate(_effect.AddComponent<Effect>());

        // 3. 데이터 추가
        effect.gameObject.name = starStunEffectName;
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

    public PlayerAttackEffect CreateAttackEffect(int ani_id)
    {
        string effectPrefabName = string.Empty;

        switch (ani_id)
        {
            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_02:
            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_03:
                effectPrefabName = attackEffectName;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                effectPrefabName = eSkillAttackEffectName;
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                effectPrefabName = qSkillAttackEffectName;
                break;
        }

        // 이펙트 풀에 이미 해당 노드에 비활성화 된 이펙트가 있는지 검사
        Effect _existEffect = effects.Find(eff => eff.gameObject.name == effectPrefabName && eff.self.isStopped);
        PlayerAttackEffect existEffect = (PlayerAttackEffect)_existEffect;

        if (existEffect != null)
        {
            // 해당 공격에 맞게 이펙트 조정 및 데이터 추가
            AttackEffectSetup(existEffect, effectPrefabName, ani_id);
            return existEffect;
        }

        // 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + effectPrefabName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerAttackEffect eff = effect.AddComponent<PlayerAttackEffect>();
        
        // 새로 생긴 이펙트를 이펙트 풀에 추가 및 부모 지정
        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        // 해당 공격에 맞게 이펙트 조정 및 데이터 추가
        AttackEffectSetup(eff, effectPrefabName, ani_id);

        // 해당 이펙트 return
        return eff;
    }

    public PlayerHitEffect CreateHitEffect(int ani_id, Monster hitMob)
    {
        string effectPrefabName = string.Empty;

        switch (ani_id)
        {
            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                effectPrefabName = qSkillHitEffectName;
                break;

            default:
                effectPrefabName = hitEffectName;
                break;
        }

        Effect _existEffect = effects.Find(eff => eff.gameObject.name == effectPrefabName && !eff.gameObject.activeSelf);
        PlayerHitEffect existEffect = (PlayerHitEffect)_existEffect;

        if (existEffect != null)
        {
            existEffect.gameObject.SetActive(true);
            existEffect._hitLight.gameObject.SetActive(true);
            existEffect.transform.SetParent(this.transform);
            existEffect.transform.position = SetHitEffectPosition(hitMob);
            return existEffect;
        }

        GameObject _effect = Resources.Load<GameObject>("Particle/" + effectPrefabName);
        GameObject effect = Instantiate<GameObject>(_effect);
        PlayerHitEffect eff;

        switch (ani_id)
        {
            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                eff = effect.AddComponent<PlayerQSkillHitEffect>();
                break;

            default:
                eff = effect.AddComponent<PlayerAttackHitEffect>();
                break;
        }

        effects.Add(eff);

        eff.effectNode = hitMob.transform;
        eff.gameObject.name = effectPrefabName;
        effect.transform.SetParent(this.transform);
        effect.transform.position = SetHitEffectPosition(hitMob);

        return eff;
    }

    public PlayerESkillBackEffect CreateESkillBackEffect()
    {
        string prefabName = eSkillBackEffectName;

        Effect _existEffect = effects.Find(eff => eff.gameObject.name == prefabName && !eff.gameObject.activeSelf);
        PlayerESkillBackEffect existEffect = (PlayerESkillBackEffect)_existEffect;

        if (existEffect != null)
        {
            existEffect.gameObject.SetActive(true);
            BackEffectSetup(existEffect, prefabName);
            existEffect.rippleEffect.transform.localRotation = Quaternion.Euler(FX_SKILL_E_RIPPLE_ROT);
            return existEffect;
        }
            
        GameObject _effect = Resources.Load<GameObject>("Particle/" + prefabName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerESkillBackEffect eff = effect.AddComponent<PlayerESkillBackEffect>();

        BackEffectSetup(eff, prefabName);

        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        eff.rippleEffect.transform.localRotation = Quaternion.Euler(FX_SKILL_E_RIPPLE_ROT);

        return eff;
    }

    public PlayerQSkillBackEffect CreateQSkillBackEffect()
    {
        string prefabName = qSkillBackEffectName;

        Effect _existEffect = effects.Find(eff => eff.gameObject.name == prefabName && !eff.gameObject.activeSelf);
        PlayerQSkillBackEffect existEffect = (PlayerQSkillBackEffect)_existEffect;

        if (existEffect != null)
        {
            existEffect.gameObject.SetActive(true);
            BackEffectSetup(existEffect, prefabName);
            return existEffect;
        }

        GameObject _effect = Resources.Load<GameObject>("Particle/" + prefabName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerQSkillBackEffect eff = effect.AddComponent<PlayerQSkillBackEffect>();

        BackEffectSetup(eff, prefabName);

        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        return eff;
    }

    public PlayerFootstepEffect CreateFootStepEffect()
    {
        string prefabName = footStepEffectName;

        Effect _existEffect = effects.Find(eff => eff.gameObject.name == prefabName);
        PlayerFootstepEffect existEffect = (PlayerFootstepEffect)_existEffect;

        if (existEffect != null)
            return existEffect;

        GameObject _effect = Resources.Load<GameObject>("Particle/" + prefabName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerFootstepEffect eff = effect.AddComponent<PlayerFootstepEffect>();

        eff.effectNode = GameManager.instance.controller.player.transform;
        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        return eff;
    }

    public DragonFireBallEffect CreateDragonFireBallEffect(Dragon dragon)
    {
        string prefabName = fireBallEffectName;

        Effect _existEffect = effects.Find(eff => eff.gameObject.name == prefabName);
        DragonFireBallEffect existEffect = (DragonFireBallEffect)_existEffect;

        if (existEffect != null)
        {
            FireBallEffectSetup(existEffect, dragon);
            return existEffect;
        }
            

        GameObject _effect = Resources.Load<GameObject>("Particle/" + prefabName);
        GameObject effect = Instantiate<GameObject>(_effect);

        DragonFireBallEffect eff = effect.AddComponent<DragonFireBallEffect>();

        FireBallEffectSetup(eff, dragon);
        eff.effectNode = this.transform;
        effects.Add(eff);
        effect.transform.SetParent(this.transform);

        return eff;
    }

    private void AttackEffectSetup(Effect eff, string effName, int ani_id)
    {
        if (eff.effectNode == null)
        {
            eff.gameObject.name = effName;
            eff.effectNode = GameManager.instance.controller.player.transform;
        }
        
        switch (ani_id)
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

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_01:
                eff.transform.localPosition = FX_SKILL_E_POS;
                eff.transform.localRotation = Quaternion.Euler(FX_SKILL_E_ROT);
                break;

            case (int)PlayerAnimation.AniType.COMBAT_SKILL_02:
                eff.transform.localPosition = FX_SKILL_Q_POS;
                eff.transform.localRotation = Quaternion.Euler(FX_SKILL_Q_ROT);
                eff.transform.localScale = FX_SKILL_Q_SCALE;
                break;
        }
    }

    private void BackEffectSetup(Effect eff, string effName)
    {
        if (eff.effectNode == null)
        {
            eff.gameObject.name = effName;
            eff.effectNode = this.transform;
        }

        eff.transform.position = GameManager.instance.controller.player.transform.position;
        eff.transform.rotation = GameManager.instance.controller.player.transform.rotation;
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

    private void FireBallEffectSetup(Effect eff, Dragon dragon)
    {
        eff.transform.position = dragon._fireBallEffectPos.position;
    }
}
