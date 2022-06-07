using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSliderValue : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Slider minSlider;
    [SerializeField]
    private Slider maxSlider;

    public void ControlValues()
    {
        if (minSlider != null)
        {
            if (slider.value <= minSlider.value)
            {
                slider.value = minSlider.value + 1;
            }
        }
        if (maxSlider != null)
        {
            if (slider.value >= maxSlider.value)
            {
                slider.value = maxSlider.value - 1;
            }
        }    
    }
}
