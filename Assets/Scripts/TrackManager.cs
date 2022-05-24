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
    private CinemachineSmoothPath.Waypoint[] activeAltPath;

    private bool onAltTrack;
    private int currentWaypoint;
    
    void Start()
    {
        track = GetComponentInParent<CinemachineSmoothPath>();
        originalPath = track.m_Waypoints;
        activePath = originalPath;
        onAltTrack = false;
        currentWaypoint = 0;

        foreach (CinemachineSmoothPath path in alternativeTracks)
        {
            Vector3 firstWaypointPos, lastWaypointPos;
            firstWaypointPos = path.transform.TransformPoint(path.m_Waypoints[0].position);
            lastWaypointPos = path.transform.TransformPoint(path.m_Waypoints[path.m_Waypoints.Length - 1].position);
            FindWaypoint(firstWaypointPos);
            FindWaypoint(lastWaypointPos);
        }
    }

    void FindWaypoint(Vector3 waypointPos)
    {
        float previousDistance = Mathf.Infinity;
        CinemachineSmoothPath.Waypoint waypoint = new CinemachineSmoothPath.Waypoint();
        waypoint.position = track.transform.InverseTransformPoint(waypointPos);
        waypoint.roll = 0;
        for (int i = 0; i < activePath.Length; i++)
        {
            if (Vector3.Distance(transform.TransformPoint(activePath[i].position), waypointPos) >= previousDistance)
            {
                Debug.Log("Place waypoint at index [" + i + "]");
                ArrayUtility.Insert(ref activePath, i, waypoint);
                break;
            }
            previousDistance = Vector3.Distance(transform.TransformPoint(waypointPos), transform.TransformPoint(activePath[i].position));
        }
    }

    void Update()
    {
        track.m_Waypoints = activePath;
        
        currentWaypoint = (int)Mathf.Floor(cart.m_Position);
        if(cart.m_Position >= currentWaypoint + 0.95)
        {
            if (onAltTrack) CheckIfCartEnd();

            //should have an IF check to check if the player wants to go on the alternate track.
            //Currently does it automatically.
            if (!onAltTrack)
            {
                foreach (CinemachineSmoothPath path in alternativeTracks)
                {
                    if (transform.TransformPoint(activePath[currentWaypoint + 1].position) == path.transform.TransformPoint(path.m_Waypoints[0].position)) AAAAAAAAAAAAH(path);
                }
            }
        }
    }

    void CheckIfCartEnd() { 
        if(Mathf.Round(cart.m_Position) == activeAltPath.Length - 1)
        {
            onAltTrack = false;
            activeAltPath = null;
        }
    }

    void AAAAAAAAAAAAH(CinemachineSmoothPath switchingPath) {
        onAltTrack = true;
        int firstPointIndex = 0; int endPointIndex = 0;

        for(int i = 0; i < activePath.Length; i++)
        {
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[0].position)) 
                firstPointIndex = i;
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[switchingPath.m_Waypoints.Length - 1].position)) 
                endPointIndex = i;
        }
        Debug.Log("First: " + firstPointIndex);
        Debug.Log("End: " + endPointIndex);

        CinemachineSmoothPath.Waypoint[] temp = new CinemachineSmoothPath.Waypoint[0];

        for (int i = 0; i < activePath.Length; i++)
        {
            if (i > firstPointIndex && i < endPointIndex)
            { 
                continue;
            }
            if (i == firstPointIndex)
            {
                Debug.Log("Started adding waypoints at: " + i);
                for (int x = 0; x < switchingPath.m_Waypoints.Length; x++)
                {
                    switchingPath.m_Waypoints[x].position = switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[x].position);
                    switchingPath.m_Waypoints[x].position = track.transform.InverseTransformPoint(switchingPath.m_Waypoints[x].position);
                }
                ArrayUtility.AddRange(ref temp, switchingPath.m_Waypoints);
            }
            if (i != firstPointIndex && i != endPointIndex) ArrayUtility.Add(ref temp, activePath[i]);
        }

        activeAltPath = switchingPath.m_Waypoints;
        activePath = temp;

    }

}
