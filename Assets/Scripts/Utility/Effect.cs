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
        ps = GetComponent<ParticleSystem>();
    }

}

public class StunEffect : Effect
{
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
}
