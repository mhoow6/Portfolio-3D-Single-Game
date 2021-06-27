using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public ushort id;
    public float attack_damage;
    public float attack_distance;
    public float attack_angle;
    public float walk_speed;

    [SerializeField]
    protected float currentDistanceWithPlayer;
    [SerializeField]
    protected float currentAngleWithPlayer;

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

                if (renderer != null)
                {
                    // 3. Get bounds
                    bound = renderer.bounds;

                    // 4. Avoid Root Object
                    return bound;
                }
            }
        }

        return null;
    }

    protected void BoundUpdate(bool isBoundStatic)
    {
        if (!isBoundStatic)
            bound.center = transform.position;
    }

    protected virtual void OnBoundEnter(Character collided)
    {
        if (collided != null)
            Debug.Log("Bound Intersects with " + collided.name);
    }

    protected virtual void OnBoundEscape()
    {
        Debug.Log("Bound Escape.");
    }

    protected void Detector()
    {
        currentDistanceWithPlayer = Vector3.Distance(GameManager.instance.controller.player.transform.position, transform.position);
        currentAngleWithPlayer = Mathf.Acos(Vector3.Dot
                (transform.forward,
                (GameManager.instance.controller.player.transform.position - transform.position).normalized)
                ) * Mathf.Rad2Deg;
    }
}
