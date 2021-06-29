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

    private const string attackEffectName = "FX_NormalAttack";
    private ushort attackEffectID;
    private const string starStunEffectName = "FX_StarStunned_01";
    private ushort starStunEffectID;
    private const string hitEffectName = "FX_NormalHit";
    private ushort hitEffectID;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        attackEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(attackEffectName);
        starStunEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(starStunEffectName);
        hitEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(hitEffectName);
    }

    public Effect CreateStarStunEffect(ushort objID, Transform head)
    {

        // 1. 이펙트 풀에 이미 해당 위치에 비활성화 된 이펙트가 있는지 검사
        Effect existEffect = effects.Find(eff => eff.effectNode == head && eff.effectID == starStunEffectID && !eff.ps.isPlaying);

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
        PlayerAttackEffect existEffect = (PlayerAttackEffect)effects.Find(eff => eff.effectID == attackEffectID && !eff.ps.isPlaying);

        if (existEffect != null)
        {
            // 해당 공격에 맞게 이펙트 조정
            AttackEffectTransformSetup(existEffect, aniIndex);
            return existEffect;
        }

        // 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + attackEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerAttackEffect eff = effect.AddComponent<PlayerAttackEffect>();
        
        // 새로 생긴 이펙트를 이펙트 풀에 추가 및 부모 지정
        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        // 해당 공격에 맞게 이펙트 조정
        AttackEffectTransformSetup(eff, aniIndex);

        // 해당 이펙트 return
        return eff;
    }

    public PlayerAttackHitEffect CreateHitEffect(Monster hitMob)
    {
        // 이펙트 풀에 이미 해당 노드에 비활성화 된 이펙트가 있는지 검사
        PlayerAttackHitEffect existEffect = (PlayerAttackHitEffect)effects.Find(eff => eff.effectID == attackEffectID && eff.effectNode == hitMob.transform && !eff.ps.isPlaying);

        if (existEffect != null)
        {
            // 해당 공격에 맞게 이펙트 위치 조정
            existEffect.transform.position = HitEffectPositionSetup(hitMob, GameManager.instance.controller.player.transform);
            return existEffect;
        }

        // 그게 아니면 새로 이펙트 생성
        GameObject _effect = Resources.Load<GameObject>("Particle/" + attackEffectName);
        GameObject effect = Instantiate<GameObject>(_effect);

        PlayerAttackHitEffect eff = effect.AddComponent<PlayerAttackHitEffect>();

        // 새로 생긴 이펙트를 이펙트 풀에 추가 및 부모 지정
        effects.Add(eff);
        effect.transform.SetParent(GameManager.instance.controller.player.transform);

        // 해당 공격에 맞게 이펙트 조정
        effect.transform.position = HitEffectPositionSetup(hitMob, GameManager.instance.controller.player.transform);

        // 해당 이펙트 return
        return eff;
    }

    private void AttackEffectTransformSetup(Effect eff, int aniIndex)
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

    private Vector3 HitEffectPositionSetup(Monster hitMob, Transform player)
    {
        float minDistance = Vector3.Distance(player.position, hitMob.hitEffectsPos[0].position);
        Vector3 effectPos = hitMob.hitEffectsPos[0].position;

        for (int i = 1; i < hitMob.hitEffectsPos.Length; i++)
        {
            float dis = Vector3.Distance(player.position, hitMob.hitEffectsPos[i].position);

            if (dis < minDistance)
            {
                minDistance = dis;
                effectPos = hitMob.hitEffectsPos[i].position;
            }
        }

        return effectPos;
    }

}
