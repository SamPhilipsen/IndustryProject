using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;
    [SerializeField] private CinemachineDollyCart cart;
    [SerializeField] private CinemachineSmoothPath[] alternativeTracks;
    private CinemachineSmoothPath track;
    private CinemachineSmoothPath.Waypoint[] activePath;
    private CinemachineSmoothPath.Waypoint[] originalPath;
    private bool onAltTrack;
    
    void Start()
    {
        track = GetComponentInParent<CinemachineSmoothPath>();
        originalPath = track.m_Waypoints;
        activePath = originalPath;
        onAltTrack = false;
    }

    void Update()
    {
        track.m_Waypoints = activePath;
        CheckIfCartReachedWaypoint();
        CheckIfCartEnd();
    }

    void CheckIfCartReachedWaypoint()
    {
        foreach(CinemachineSmoothPath path in alternativeTracks)
        {
            if(Vector3.Distance(transform.TransformPoint(path.m_Waypoints[0].position), cart.transform.position) <= 1)
            {
                if(!onAltTrack)
                    AAAAAAAAAAAAH();
            }
        }
    }
    void CheckIfCartEnd() { 
        if(cart.transform.position == transform.TransformPoint(activePath[activePath.Length - 1].position))
        {
            activePath = originalPath;
            onAltTrack = false;
        }
    }

    void AAAAAAAAAAAAH() {
        onAltTrack = true;
        int firstPointIndex = 0; int endPointIndex = 0;

        for(int i = 0; i < activePath.Length; i++)
        {
            if (activePath[i].position == alternativeTracks[0].m_Waypoints[0].position) firstPointIndex = i;
            if (activePath[i].position == alternativeTracks[0].m_Waypoints[alternativeTracks[0].m_Waypoints.Length - 1].position) endPointIndex = i;
        }
        Debug.Log("First: " + firstPointIndex);
        Debug.Log("End: " + endPointIndex);

        // Debug.Log(alternativeTracks[0].m_Waypoints[0].position);
        //Debug.Log(originalPath[1].position);

        /*ArrayUtility.RemoveAt(ref alternativeTracks[0].m_Waypoints, 0);
        ArrayUtility.RemoveAt(ref alternativeTracks[0].m_Waypoints, alternativeTracks[0].m_Waypoints.Length - 1);*/

        CinemachineSmoothPath.Waypoint[] temp = new CinemachineSmoothPath.Waypoint[0];

        for (int i = 0; i < activePath.Length; i++)
        {
            if (i > firstPointIndex && i < endPointIndex)
            {
                continue;
            }
            if (i == firstPointIndex)
            {
                ArrayUtility.AddRange(ref temp, alternativeTracks[0].m_Waypoints);
            }
            if (i != firstPointIndex && i != endPointIndex) ArrayUtility.Add(ref temp, activePath[i]);
        }

        activePath = temp;

    }

}
