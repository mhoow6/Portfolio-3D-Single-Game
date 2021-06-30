using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem ps;
    public Transform effectNode;
    public ushort effectID;

    private void Awake()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }

    protected void GetChildParticleSystem(Transform parent, string particeName, ref ParticleSystem node)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            // �̹� nodeName�� �´� ���� ã�Ƽ� null�� �ƴ� ��� �ǹ̾��� ȣ���� �����ϱ� ���� �Լ� ����
            if (node != null)
                return;

            Transform child = parent.GetChild(i);

            if (child.name != particeName)
            {
                // �ڽ��� �Ǵٸ� �ڽ��� ���� ��� �ڽ��� �ڽĵ��� Ž��
                if (child.childCount != 0)
                    GetChildParticleSystem(child, particeName, ref node);
            }

            if (child.name == particeName)
                node = child.GetComponent<ParticleSystem>();
        }
    }

    protected virtual IEnumerator PlayCheckUpdate()
    {
        yield return null;
    }
}

public class PlayerAttackEffect : Effect
{
    private void Awake()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }
}

public class PlayerAttackHitEffect : Effect
{
    public List<ParticleSystem> hitEffects = new List<ParticleSystem>();
    public Light hitLight;

    private WaitForSeconds hitChangeColorWt = new WaitForSeconds(0.2f);
    private Color hitChangeColor = new Color(0.117f, 0, 0, 1f); // RGB 30, 0, 0

    private void Awake()
    {
        effectNode = EffectManager.instance.transform;
        effectID = EffectManager.instance.hitEffectID;

        if (hitEffects.Count == 0)
        {
            ParticleSystem _ps;

            for (int i=0; i < transform.childCount; i++)
            {
                _ps = transform.GetChild(i).GetComponent<ParticleSystem>();

                if (_ps != null)
                    hitEffects.Add(_ps);

                if (ps == null)
                    hitLight = transform.GetChild(i).GetComponent<Light>();
            }
        }
    }

    public void PlayEffects(Monster hitMob)
    {
        for (int i = 0; i < hitEffects.Count; i++)
            hitEffects[i].Play(true);

        StartCoroutine(PlayCheckUpdate());
        StartCoroutine(LightUpdate());
        StartCoroutine(HitChangeMobColor(hitMob));
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (hitEffects[0].isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator LightUpdate()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;

            if (time >= 0.1f)
            {
                hitLight.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator HitChangeMobColor(Monster hitMob)
    {
        hitMob.smr.material.EnableKeyword("_EMISSION");
        hitMob.smr.material.SetColor("_EmissionColor", hitChangeColor);
        
        yield return hitChangeColorWt;

        hitMob.smr.material.DisableKeyword("_EMISSION");
        hitMob.smr.material.SetColor("_EmissionColor", hitMob.originEmissionColor);
    }
}

public class PlayerESkillAttackEffect : Effect
{
    private void Awake()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }
}

public class PlayerESkillBackEffect : Effect
{
    private ParticleSystem swirlEffect;
    public ParticleSystem rippleEffect;
    private WaitForSeconds rippleStayWaitTime = new WaitForSeconds(0.5f);

    private void Awake()
    {
        GetChildParticleSystem(transform, "FX_Swirl_Fast_01", ref swirlEffect);
        GetChildParticleSystem(transform, "FX_Water_Ripple", ref rippleEffect);
    }

    public void PlayEffect()
    {
        swirlEffect.Play(true);
        rippleEffect.Play(true);
    }

    public void StopSwirlEffect()
    {
        // ��ƼŬ �߻길 ����. �Լ� ȣ���ϴ� ���� ��ƼŬ�� ��� �縮���� �ʰ� �� �� ����
        swirlEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void StopRippleEffect()
    {
        rippleEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(PlayCheckUpdate());
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (rippleEffect.isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}
