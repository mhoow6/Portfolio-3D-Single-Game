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

    private void Awake()
    {
        instance = this;
    }

    public Effect CreateStarStunEffect(ushort objID, Transform head)
    {
        string starStunEffectName = "FX_StarStunned_01";
        ushort starStunEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(starStunEffectName);

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

}
