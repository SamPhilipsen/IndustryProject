using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementCommunicator : MonoBehaviour
{
    [SerializeField] Offset offsetScript;
    [SerializeField] TrackManager trackManager;
    [SerializeField] Canvas canvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (offsetScript.newPositionPlayer.x >= 2) trackManager.switchingTracks = "right";
        else if (offsetScript.newPositionPlayer.x <= -2) trackManager.switchingTracks = "left";
        else trackManager.switchingTracks = "forward";

        if(offsetScript.newPositionPlayer.x >= 2)
        {
            //GameObject.Find("Canvas").transform.GetChild("LeftArrow");
        }

        //canvas.GetComponentInChildren<RawImage>().color = new Color(255, 255, 0);
    }
}
