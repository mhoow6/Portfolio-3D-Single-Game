using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float attack_damage;
    public float attack_distance;
    public float attack_angle;
    public float walk_speed;
    public Bounds _bound
    {
        get => bound;
    }

    protected Bounds bound;

    protected Bounds? GetBoundFromSkinnedMeshRenderer(Character character)
    {
        Transform trans = character.transform;
        Bounds bound;

        // 1. Get SkinnedMeshRenderer in GameObject
        SkinnedMeshRenderer renderer = null;

        // 2. Active gameobject is myself.
        for (int i = 0; i < trans.childCount; i++)
        {
            if (trans.GetChild(i).gameObject.activeSelf)
            {
                renderer = trans.GetChild(i).GetComponent<SkinnedMeshRenderer>();

                // 3. Get bounds
                bound = renderer.bounds;

                // 4. Avoid Root Object
                return bound;
            }
        }

        return null;
    }

    protected void BoundUpdate(Character character, bool isBoundStatic)
    {
        if (!isBoundStatic)
            this.bound.center = transform.position;

        if (character != null)
        {
            if (bound.Intersects(character._bound))
                OnBoundEnter();
        }
    }

    protected virtual void OnBoundEnter()
    {
        Debug.Log("Bound Intersects.");
    }
}
