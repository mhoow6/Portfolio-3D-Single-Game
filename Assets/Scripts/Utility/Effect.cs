using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem self;
    public Transform effectNode;

    private void Awake()
    {
        if (self == null)
            self = GetComponent<ParticleSystem>();
    }

    protected void GetChildParticleSystem(Transform parent, string particeName, ref ParticleSystem node)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            // 이미 nodeName에 맞는 것을 찾아서 null이 아닐 경우 의미없는 호출을 방지하기 위해 함수 종료
            if (node != null)
                return;

            Transform child = parent.GetChild(i);

            if (child.name != particeName)
            {
                // 자식이 또다른 자식을 가질 경우 자식의 자식들을 탐색
                if (child.childCount != 0)
                    GetChildParticleSystem(child, particeName, ref node);
            }

            if (child.name == particeName)
                node = child.GetComponent<ParticleSystem>();
        }
    }

    protected void GetChildLight(Transform parent, string lightName, ref Light node)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (node != null)
                return;

            Transform child = parent.GetChild(i);

            if (child.name != lightName)
            {
                if (child.childCount != 0)
                    GetChildLight(child, lightName, ref node);
            }

            if (child.name == lightName)
                node = child.GetComponent<Light>();
        }
    }

    protected virtual IEnumerator PlayCheckUpdate()
    {
        yield return null;
    }

    protected void GetNodeObject(Transform parent, string nodeName, ref Transform node)
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

public class PlayerAttackEffect : Effect
{
    private void Awake()
    {
        if (self == null)
            self = GetComponent<ParticleSystem>();
    }
}

public class HitEffect : Effect
{
    protected Light hitLight;
    public Light _hitLight { get => hitLight; }

    protected IEnumerator LightUpdate()
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
}

public class PlayerHitEffect : HitEffect
{
    private WaitForSeconds hitChangeColorWt = new WaitForSeconds(0.2f);
    private Color hitChangeColor = new Color(0.117f, 0, 0, 1f); // RGB 30, 0, 0

    public void PlayEffects(Monster hitMob)
    {
        Play(hitMob);
    }

    protected virtual void Play(Monster hitMob) { }

    protected IEnumerator HitChangeMobColor(Monster hitMob)
    {
        hitMob.smr.material.EnableKeyword("_EMISSION");
        hitMob.smr.material.SetColor("_EmissionColor", hitChangeColor);

        yield return hitChangeColorWt;

        hitMob.smr.material.DisableKeyword("_EMISSION");
        hitMob.smr.material.SetColor("_EmissionColor", hitMob.originEmissionColor);
    }
}

public class PlayerAttackHitEffect : PlayerHitEffect
{
    private List<ParticleSystem> hitEffects = new List<ParticleSystem>();

    private void Awake()
    {
        if (hitEffects.Count == 0)
        {
            ParticleSystem _ps;

            for (int i = 0; i < transform.childCount; i++)
            {
                _ps = transform.GetChild(i).GetComponent<ParticleSystem>();

                if (_ps != null)
                    hitEffects.Add(_ps);

                if (_ps == null)
                    hitLight = transform.GetChild(i).GetComponent<Light>();
            }
        }
    }

    protected override void Play(Monster hitMob)
    {
        for (int i = 0; i < hitEffects.Count; i++)
            hitEffects[i].Play(true);

        StartCoroutine(PlayCheckUpdate());
        StartCoroutine(LightUpdate());
        // StartCoroutine(HitChangeMobColor(hitMob));
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
}

public class PlayerQSkillHitEffect : PlayerHitEffect
{
    private ParticleSystem hitEffect;

    private void Awake()
    {
        GetChildParticleSystem(transform, "FX_Fire_Explosion_01", ref hitEffect);
        GetChildLight(transform, "Hit_Light", ref hitLight);
    }

    protected override void Play(Monster hitMob)
    {
        hitEffect.Play(true);

        StartCoroutine(PlayCheckUpdate());
        StartCoroutine(LightUpdate());
        StartCoroutine(HitChangeMobColor(hitMob));
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (hitEffect.isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}

public class PlayerSkillBackEffect : Effect
{
    public void PlayEffect()
    {
        Play();
    }

    public void StopEffect()
    {
        Stop();
        StartCoroutine(PlayCheckUpdate());
    }

    protected virtual void Play() { }
    protected virtual void Stop() { }

}

public class PlayerESkillBackEffect : PlayerSkillBackEffect
{
    public ParticleSystem rippleEffect;
    private ParticleSystem swirlEffect;

    private void Awake()
    {
        GetChildParticleSystem(transform, "FX_Swirl_Fast_01", ref swirlEffect);
        GetChildParticleSystem(transform, "FX_Water_Ripple", ref rippleEffect);
    }

    protected override void Play()
    {
        swirlEffect.Play(true);
        rippleEffect.Play(true);
    }

    protected override void Stop()
    {
        // 파티클 발산만 중지. 함수 호출하는 순간 파티클이 즉시 사리자지 않게 할 수 있음
        swirlEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        rippleEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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

public class PlayerQSkillBackEffect : PlayerSkillBackEffect
{
    private ParticleSystem ember;
    private ParticleSystem swirl;
    private Light front_light;
    private Light back_light;

    private const float LIGHT_TURN_OFF_SPEED = 1f;

    private void Awake()
    {
        GetChildParticleSystem(this.transform, "FX_Embers_01", ref ember);
        GetChildParticleSystem(this.transform, "FX_Swirl_03", ref swirl);
        GetChildLight(this.transform, "Back_Light", ref back_light);
        GetChildLight(this.transform, "Front_Light", ref front_light);
    }

    protected override void Play()
    {
        ember.Play(true);
        swirl.Play(true);

        front_light.intensity = 1.0f;
        back_light.intensity = 1.0f;

        front_light.gameObject.SetActive(true);
        back_light.gameObject.SetActive(true);
    }

    protected override void Stop()
    {
        ember.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        swirl.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(LightTurnoffSmooth(front_light));
        StartCoroutine(LightTurnoffSmooth(back_light));
    }

    private IEnumerator LightTurnoffSmooth(Light light)
    {
        while (true)
        {
            light.intensity = Mathf.Lerp(light.intensity, 0f, Time.deltaTime * LIGHT_TURN_OFF_SPEED);

            if (light.intensity <= 0.1f)
            {
                light.intensity = 0f;
                yield break;
            }

            yield return null;
        }
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (ember.isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}

public class PlayerFootstepEffect : Effect
{
    private ParticleSystem.EmissionModule em;
    
    private void Awake()
    {
        if (self == null)
            self = GetComponent<ParticleSystem>();

        em = self.emission;
    }

    public void FootStepChange(PlayerAnimation.AniType ani_id)
    {
        switch (ani_id)
        {
            case PlayerAnimation.AniType.WALK:
            case PlayerAnimation.AniType.COMBAT_WALK:
                em.enabled = true;
                em.rateOverDistance = 0f;
                break;
            case PlayerAnimation.AniType.RUN:
                em.enabled = true;
                em.rateOverDistance = 1.0f;
                break;
            default:
                em.enabled = false;
                break;
        }
    }
}

public class DragonFireBallEffect : Effect
{
    public Bounds _bound { get => bound; }
    public float currentDamage;

    [SerializeField]
    private Bounds bound;

    private float fireBallSize;
    private Vector3 originPos;
    private float originToTargetDistance;
    private Vector3 originToTargetDirection;
    private Vector3 targetPos;
    private float deltaDistance;
    private const float FIRE_BALL_MOVE_SPEED = 15.0f;
    private const float METEO_MOVE_SPEED = 10.0f;
    private Vector3 PLAYER_BODY_POS = new Vector3(0, 0.5f, 0);

    private void Awake()
    {
        if (self == null)
            self = GetComponent<ParticleSystem>();

        BoundSetup();
    }

    private void OnEnable()
    {
        StartCoroutine(OnBoundIntersect());
    }

    private void Update()
    {
        bound.center = transform.position;
    }

    private void BoundSetup()
    {
        bound.center = transform.position;

        fireBallSize = self.main.startSize.constant * 0.2f;

        bound.extents = new Vector3(fireBallSize, fireBallSize, fireBallSize);
    }

    private IEnumerator OnBoundIntersect()
    {
        while (true)
        {
            yield return null; // 최초 OnEnable 시 transform는 Update()가 끝난 뒤에 업데이트되므로 먼저 yield return을 해준다.

            if (bound.Intersects(GameManager.instance.controller.player._bound))
            {
                EffectManager.instance.CreateDragonFireBallHitEffect(transform.position, gameObject.name).PlayEffect();
                GameManager.instance.controller.player.currentHp -= currentDamage;
                self.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                StartCoroutine(PlayCheckUpdate());
                yield break;
            }

            if (GameManager.instance.area != null)
                if (bound.Intersects(GameManager.instance.area.BOUND))
                {
                    EffectManager.instance.CreateDragonFireBallHitEffect(transform.position, gameObject.name).PlayEffect();
                    self.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    StartCoroutine(PlayCheckUpdate());
                    yield break;
                }
        }
    }

    public void PlayEffect(int ani_id, Dragon dragon)
    {
        self.Play(true);

        switch (ani_id)
        {
            case (int)DragonAnimation.AniType.FIREBALL:
                StartCoroutine(MoveFireBall(dragon._fireBallDistance, FIRE_BALL_MOVE_SPEED));
                break;
            case (int)DragonAnimation.ComboAniType.FLY_FIREBALL:
                StartCoroutine(MoveFireBall(dragon._flyAttackDistance, FIRE_BALL_MOVE_SPEED));
                break;
            case (int)DragonAnimation.ComboAniType.METEO:
                StartCoroutine(MoveFireBall(dragon._flyAttackDistance, METEO_MOVE_SPEED));
                break;
        }
    }

    private IEnumerator MoveFireBall(float moveDistance, float moveSpeed)
    {
        originPos = transform.position;

        originToTargetDistance = Vector3.Distance(originPos, GameManager.instance.controller.cameraArm.transform.position);
        originToTargetDirection = (GameManager.instance.controller.cameraArm.transform.position - originPos).normalized;

        targetPos = GameManager.instance.controller.cameraArm.transform.position + (originToTargetDirection * (moveDistance - originToTargetDistance));

        while (deltaDistance < moveDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            deltaDistance = Vector3.Distance(originPos, transform.position);
            yield return null;
        }

        self.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(PlayCheckUpdate());
        deltaDistance = 0f;
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (self.isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}

public class DragonFireBallHitEffect : HitEffect
{
    private const float LIGHT_TURN_OFF_SPEED = 5.0f;

    private void Awake()
    {
        if (self == null)
            self = GetComponent<ParticleSystem>();

        GetChildLight(this.transform, "Explosion_Light", ref hitLight);
    }

    public void PlayEffect()
    {
        self.Play(true);
        hitLight.intensity = 1.0f;
        StartCoroutine(LightTurnoffSmooth(hitLight));

        Invoke("StopEffect", 1f);
    }

    public void StopEffect()
    {
        self.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StartCoroutine(PlayCheckUpdate());
    }

    private IEnumerator LightTurnoffSmooth(Light light)
    {
        while (true)
        {
            light.intensity = Mathf.Lerp(light.intensity, 0f, Time.deltaTime * LIGHT_TURN_OFF_SPEED);

            if (light.intensity <= 0.1f)
            {
                light.intensity = 0f;
                yield break;
            }

            yield return null;
        }
    }

    protected override IEnumerator PlayCheckUpdate()
    {
        while (true)
        {
            if (self.isStopped)
            {
                this.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}
