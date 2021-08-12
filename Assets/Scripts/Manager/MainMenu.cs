using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public Title title;
    public PatchManager patch;
    public NetworkManager network;

    private AudioSource mainBGM;

    private void Awake()
    {
        instance = this;
        SceneInfoManager.instance.currentScene = SceneType.Menu;
    }

    private void Start()
    {
        mainBGM = AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_MENU, AudioCondition.SCENE_AWAKE));
        AudioManager.instance.PlayAudioFadeIn(mainBGM, 1f);
    }
}
