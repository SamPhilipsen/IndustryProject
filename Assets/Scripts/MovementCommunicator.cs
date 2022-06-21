using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class MovementCommunicator : MonoBehaviour
{
    [SerializeField] Offset offsetScript;
    [SerializeField] TrackManager trackManager;
    [SerializeField] Canvas canvas;

    private string offsetSide;
    private CinemachineSmoothPath currentPath;
    private bool nearingSwitch;
    void Start()
    {
        trackManager.nearingSwitch += NearingSwitch;
        canvas.gameObject.SetActive(false);
    }

    void NearingSwitch(bool nearingSwitch, CinemachineSmoothPath path)
    {
        this.nearingSwitch = nearingSwitch;
        this.currentPath = path;
    }
    void Update()
    {
        if (nearingSwitch)
        {
            trackManager.switchingTracks = offsetSide;

            if (offsetScript.newPositionPlayer.x > 0) offsetSide = "right";
            else if (offsetScript.newPositionPlayer.x <= 0) offsetSide = "left";

            currentPath.GetComponent<TrackSideController>().ActivateArrow(offsetSide);
        }
    }
}
