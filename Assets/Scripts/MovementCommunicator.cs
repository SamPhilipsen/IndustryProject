using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class MovementCommunicator : MonoBehaviour
{
    [SerializeField] Offset offsetScript;
    [SerializeField] TrackManager trackManager;

    private string offsetSide;
    private CinemachineSmoothPath currentPath;
    private bool nearingSwitch;
    void Start()
    {
        trackManager.nearingSwitch += NearingSwitch;
    }

    void NearingSwitch(CinemachineSmoothPath path)
    {
        if (path is null) return;
        this.currentPath = path;
    }
    void Update()
    {
        trackManager.switchingTracks = offsetSide;

        if (offsetScript.newPositionPlayer.x > 0) offsetSide = "right";
        else if (offsetScript.newPositionPlayer.x <= 0) offsetSide = "left";

        currentPath.GetComponent<TrackSideController>().ActivateArrow(offsetSide);
    }
}
