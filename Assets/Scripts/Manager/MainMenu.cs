using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public Title title;
    public PatchManager patch;

    private void Awake()
    {
        instance = this;
    }
}
