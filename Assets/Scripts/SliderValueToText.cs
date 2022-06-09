using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueToText : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text text;

    private void Awake()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        text.text = slider.value.ToString();
    }
}
