using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundContent : MonoBehaviour
{
    public Image soundON;
    public Image soundOFF;
    public TMP_Text soundText;
    public Slider soundSlider;
    public byte sound
    {
        get => byte.Parse(soundText.text);
    }

    private Color gray;
    private Color orange;

}
