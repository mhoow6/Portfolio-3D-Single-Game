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
    private void Awake()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }
}

public class PlayerAttackHitEffect : Effect
{
    public List<ParticleSystem> hitEffects = new List<ParticleSystem>();

    private void Awake()
    {
        if (hitEffects.Count == 0)
        {
            for (int i=0; i < transform.childCount; i++)
            {
                hitEffects.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
            }
        }
    }

}
