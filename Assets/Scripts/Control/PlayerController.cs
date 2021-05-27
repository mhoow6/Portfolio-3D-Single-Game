using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraArm;
    public Player player;
    public bool immobile;
    public bool isPlayerWantToMove;
    public bool isPlayerWantToRun;
    public bool isPlayerWantToRoll;
    
    private Vector2 moveInput;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    //private Vector3 moveVector;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Vector3 moveVector;

    private void Awake()
    {
        immobile = false;
        isPlayerWantToMove = false;
        isPlayerWantToRun = false;
        isPlayerWantToRoll = false;
    }

    private void Start()
    {
        GameManager.instance.controller = this;
        moveSpeed = PlayerInfoTableManager.playerInfo.walk_speed;
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (moveInput.magnitude != 0)
            isPlayerWantToMove = true;
        else
            isPlayerWantToMove = false;

        if (immobile)
            moveInput = Vector3.zero;

        if (isPlayerWantToRun)
            moveSpeed = player.run_speed;
        else
            moveSpeed = player.walk_speed;
            
        if (player.isCombatMode)
            moveSpeed = PlayerInfoTableManager.playerInfo.combat_walk_speed;

        cameraForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        cameraRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        moveVector = cameraForward * moveInput.y + cameraRight * moveInput.x;

        if (moveVector.magnitude != 0)
            player.transform.forward = moveVector.normalized;

        if (isPlayerWantToRoll)
            transform.position += (player.transform.forward * PlayerInfoTableManager.playerInfo.roll_distance * Time.deltaTime * moveSpeed);
        else
            transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
}
