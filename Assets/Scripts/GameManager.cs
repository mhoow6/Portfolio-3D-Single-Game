using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController controller;

    public List<Monster> monsters = new List<Monster>();

    private void Awake()
    {
        instance = this;
    }
}
