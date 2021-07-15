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
