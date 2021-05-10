using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraArm;

    [SerializeField]
    private float moveSpeed = 3.0f;

    public Player player;

    public bool immobile;
    public Vector3 moveVector;
    public bool isCombatMode;

    private void Awake()
    {
        immobile = false;
        isCombatMode = false;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput;

        if (!immobile)
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else
            moveInput = Vector3.zero;

        Vector3 cameraForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        moveVector = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (moveVector.magnitude != 0)
            player.transform.forward = moveVector.normalized;

        if (isCombatMode)
            moveSpeed = 1.0f;
        else
            moveSpeed = 3.0f;

        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
}
