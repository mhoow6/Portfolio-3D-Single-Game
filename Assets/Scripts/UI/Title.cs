using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    public Image titleBackground;
    public Image titleImage;

    public Image buttonBackground;
    public GameButton bStart;
    public GameButton bQuit;

    public void GameQuit()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.GetAudio(AudioCondition.SCENE_MENU, AudioCondition.SCENE_MENU_GAME_START), AudioManager.instance._GAME_START_SOUND);
        Invoke("GoVillage", 1.0f);
    }

    public void ButtonFadeOut()
    {
        buttonBackground.gameObject.SetActive(true);
        StartCoroutine(Utility.AlphaBlending(buttonBackground, 1, 1f));
        StartCoroutine(Utility.AlphaBlending(bStart.buttonText, 1, 1f));
        StartCoroutine(Utility.AlphaBlending(bQuit.buttonText, 1, 1f));
    }

    private void GoVillage()
    {
        SceneManager.LoadScene((int)SceneType.Village);
    }
}
