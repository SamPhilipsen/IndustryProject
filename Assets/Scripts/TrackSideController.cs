using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSideController : MonoBehaviour
{
    [SerializeField] public string trackSide;

    void Start()
    {
        trackSide = trackSide.ToLower();
    }
}
