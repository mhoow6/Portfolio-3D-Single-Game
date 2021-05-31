using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController controller;
    public List<Monster> monsters = new List<Monster>();
    public List<NPC> npcs = new List<NPC>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // CursorLocking();
    }

    void CursorLocking()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
