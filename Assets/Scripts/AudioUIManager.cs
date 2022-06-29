using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIManager : MonoBehaviour
{
    [SerializeField] GameObject mainButtons;
    [SerializeField] GameObject audioButtons;

    [SerializeField] List<AudioSource> effectSources;
    [SerializeField] AudioSource backgroundSource;

    [SerializeField] Slider effectSlider;
    [SerializeField] Slider backgroundSlider;

    public void LoadAudioMenu()
    {
        mainButtons.SetActive(false);
        audioButtons.SetActive(true);
    }
    public void HideAudioMenu()
    {
        audioButtons.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void SaveSound()
    {
        foreach (var item in effectSources)
        {
            item.volume = effectSlider.value;
        }
        backgroundSource.volume = backgroundSlider.value;
    }
}
