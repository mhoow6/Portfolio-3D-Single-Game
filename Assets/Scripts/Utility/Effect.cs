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
}

public class PlayerAttackEffect : Effect
{
    public Transform hitEffect;

    private void Awake()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        if (hitEffect == null)
            StartCoroutine(GetFXHitTransform());
    }

    IEnumerator GetFXHitTransform()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "FX_Hit")
            {
                hitEffect = transform.GetChild(i);
                yield return null;
            }
        }
    }
}
