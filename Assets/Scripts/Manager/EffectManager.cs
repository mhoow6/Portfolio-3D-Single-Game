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
    private Vector3 FX_NORMAL_COMBAT_ATTACK_01_POS = new Vector3(0.97f, 1.507f, -0.304f);
    private Vector3 FX_NORMAL_COMBAT_ATTACK_01_ROT = new Vector3(-34.76f, 128.518f, 35.615f);

    private void Awake()
    {
        instance = this;
    }

    public Effect CreateStarStunEffect(ushort objID, Transform head)
    {
        string starStunEffectName = "FX_StarStunned_01";
        ushort starStunEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(starStunEffectName);

        // 1. ����Ʈ Ǯ�� �̹� �ش� ��ġ�� ��Ȱ��ȭ �� ����Ʈ�� �ִ��� �˻�
        Effect existEffect = effects.Find(eff => eff.effectNode == head && eff.effectID == starStunEffectID && !eff.ps.isPlaying);

        if (existEffect != null)
            return existEffect;

        // 2. �װ� �ƴϸ� ���� ����Ʈ ����
        GameObject _effect = Resources.Load<GameObject>("Particle/" + starStunEffectName);
        Effect effect = Instantiate(_effect.AddComponent<Effect>());

        // 3. ������ �߰�
        effect.effectID = starStunEffectID;
        effect.effectNode = head;

        // 4. ���� ���� ����Ʈ�� ����Ʈ Ǯ�� �߰� �� �θ� ����
        effects.Add(effect);
        effect.transform.SetParent(head);

        // 5. �ش� ���Ϳ� �°� ����Ʈ ��ġ ����
        if (objID >= 1003 && objID <= 1006) // PolyTope Studio���� ���� ���� ����
        {
            effect.transform.localPosition = POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_POS;
            effect.transform.localRotation = Quaternion.Euler(POLYTOPE_STUDIO_CHARACTER_STUN_EFFECT_ROT);
        }

        if (objID >= 1000 && objID < 1002) // Polygonal Metalon���� ���� �Ź� ����
        {
            effect.transform.localPosition = POLYGONAL_METALON_SPIDER_STUN_EFFECT_POS;
            effect.transform.localRotation = Quaternion.Euler(POLYGONAL_METALON_SPIDER_STUN_EFFECT_ROT);
        }

        // 6. �ش� ����Ʈ return
        return effect;
    }

    public Effect CreateNormalAttackEffect(int aniIndex)
    {
        string normalAttackEffectName = "FX_NormalAttack";
        ushort normalAttackEffectID = EffectInfoTableManager.GetEffectIDFromPrefabName(normalAttackEffectName);

        // ����Ʈ Ǯ�� �̹� �ش� ��忡 ��Ȱ��ȭ �� ����Ʈ�� �ִ��� �˻�
        Effect existEffect = effects.Find(eff => eff.effectNode == this.transform && eff.effectID == normalAttackEffectID && !eff.ps.isPlaying);

        if (existEffect != null)
        {
            // �ش� ���ݿ� �°� ����Ʈ ����
            switch (aniIndex)
            {
                case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
                    existEffect.transform.position = GameManager.instance.controller.player.transform.position + FX_NORMAL_COMBAT_ATTACK_01_POS;
                    existEffect.transform.rotation = Quaternion.Euler(FX_NORMAL_COMBAT_ATTACK_01_ROT);
                    break;
            }

            return existEffect;
        }
            
        // �װ� �ƴϸ� ���� ����Ʈ ����
        GameObject _effect = Resources.Load<GameObject>("Particle/" + normalAttackEffectName);
        Effect effect = Instantiate(_effect.AddComponent<PlayerAttackEffect>());

        // ������ �߰�
        effect.effectID = normalAttackEffectID;
        effect.effectNode = this.transform;

        // ���� ���� ����Ʈ�� ����Ʈ Ǯ�� �߰� �� �θ� ����
        effects.Add(effect);
        effect.transform.SetParent(this.transform);

        // �ش� ���ݿ� �°� ����Ʈ ����
        switch (aniIndex)
        {
            case (int)PlayerAnimation.AniType.COMBAT_ATTACK_01:
                effect.transform.position = GameManager.instance.controller.player.transform.position + FX_NORMAL_COMBAT_ATTACK_01_POS;
                effect.transform.rotation = Quaternion.Euler(FX_NORMAL_COMBAT_ATTACK_01_ROT);
                break;
        }
        
        // �ش� ����Ʈ return
        return effect;
    }

}
