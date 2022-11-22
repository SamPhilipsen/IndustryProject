using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SideTrack : MonoBehaviour
{
    public CinemachinePath track;
    public TrackSide trackSide;
    public bool unlocked = true;
    public float transferToPos;
    public float transferBackPos;
    public CinemachinePath trackToSwitchFrom;
    public CinemachinePath trackToSwitchBackTo;
}

public enum TrackSide
{
    left,
    right,
    up,
    down
}