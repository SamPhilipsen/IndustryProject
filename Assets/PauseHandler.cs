using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    private bool canvasState;
    private float defaultTimeScale;
    public bool paused { get; set; }

    public void Awake()
    {
        defaultTimeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        canvasState = canvas.activeSelf;
        canvas.SetActive(false);
        gameObject.SetActive(true); 
        paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = defaultTimeScale;
        if (canvasState)
        {
            canvas.SetActive(true);
        }
        gameObject.SetActive(false);
        paused = false;
        Debug.Log(Time.timeScale);
    }
}
