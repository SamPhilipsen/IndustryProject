using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    private float defaultTimeScale;
    public bool paused { get; set; }

    public void Awake()
    {
        defaultTimeScale = Time.timeScale;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = defaultTimeScale;
        paused = false;
    }
}
