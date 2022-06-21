using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class TrackSideController : MonoBehaviour
{
    [SerializeField] public string trackSide;
    [SerializeField] public GameObject arrowLeft;
    [SerializeField] public GameObject arrowRight;

    private CinemachineSmoothPath path;

    void Start()
    {
        trackSide = trackSide.ToLower();
        path = GetComponent<CinemachineSmoothPath>();
    }

    public void ActivateArrow(string side)
    {
        if(side is null)
        {
            arrowLeft.GetComponent<ArrowMover>().activateArrow = false;
            arrowRight.GetComponent<ArrowMover>().activateArrow = false;
        } else if(side.ToLower() == "right")
        {
            arrowLeft.GetComponent<ArrowMover>().activateArrow = false;
            arrowRight.GetComponent<ArrowMover>().activateArrow = true;
        } else if(side.ToLower() == "left")
        {
            arrowLeft.GetComponent<ArrowMover>().activateArrow = true;
            arrowRight.GetComponent<ArrowMover>().activateArrow = false;
        }
    }


}
