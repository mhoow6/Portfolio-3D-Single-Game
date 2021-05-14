using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraArm;

    [SerializeField]
    private float moveSpeed;

    public Player player;

    public bool immobile;
    public Vector3 moveVector;
    public bool isCombatMode;
    public bool isPlayerWantToMove;

    private void Awake()
    {
        immobile = false;
        isCombatMode = false;
        isPlayerWantToMove = false;
    }

    private void Start()
    {
        GameManager.instance.controller = this;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (moveInput.magnitude != 0)
            isPlayerWantToMove = true;
        else
            isPlayerWantToMove = false;

        if (immobile)
            moveInput = Vector3.zero;

        Vector3 cameraForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 cameraRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        moveVector = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (moveVector.magnitude != 0)
            player.transform.forward = moveVector.normalized;

        if (isCombatMode)
            moveSpeed = player.combat_move_speed;

        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
}
