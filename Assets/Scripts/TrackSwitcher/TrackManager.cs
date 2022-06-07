using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    [SerializeField] bool switchingTracks;
    [SerializeField] private CinemachineDollyCart cart;
    [SerializeField] private CinemachineSmoothPath[] alternativeTracks1;

    private CinemachineSmoothPath track;

    private CinemachineSmoothPath.Waypoint[] activePath;
    private CinemachineSmoothPath.Waypoint[] originalPath;
    private CinemachineSmoothPath.Waypoint[] activeAltPath;
    private CinemachineSmoothPath[] alternativeTracks = new CinemachineSmoothPath[0];

    private bool onAltTrack;
    private int currentWaypoint;

    void Start()
    {
        track = GetComponentInParent<CinemachineSmoothPath>();

        activePath = track.m_Waypoints;
        onAltTrack = false;
        currentWaypoint = 0;

        foreach (CinemachineSmoothPath path in alternativeTracks1)
        {
            ArrayUtility.Add(ref alternativeTracks, path);
            Debug.Log(alternativeTracks);
        }

        foreach (CinemachineSmoothPath path in alternativeTracks)
        {
            Vector3 firstWaypointPos, lastWaypointPos;
            firstWaypointPos = path.transform.TransformPoint(path.m_Waypoints[0].position);
            lastWaypointPos = path.transform.TransformPoint(path.m_Waypoints[path.m_Waypoints.Length - 1].position);
            FindWaypoint(firstWaypointPos);
            FindWaypoint(lastWaypointPos);
        }
        originalPath = activePath;
    }

    void FindWaypoint(Vector3 waypointPos)
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;
        int newWaypointIndex;

        CinemachineSmoothPath.Waypoint waypoint = new CinemachineSmoothPath.Waypoint();
        waypoint.position = track.transform.InverseTransformPoint(waypointPos);
        waypoint.roll = 0;
        for (int i = 0; i < activePath.Length; i++)
        {
            float distanceBetweenPoints = Vector3.Distance(transform.TransformPoint(activePath[i].position), waypointPos);
            if (distanceBetweenPoints < closestDistance)
            {
                closestDistance = distanceBetweenPoints;
                closestIndex = i;
            }
        }

        float distanceBehindClosestPoint;
        float distanceInfrontClosestPoint;

        try
        {
            distanceBehindClosestPoint = Vector3.Distance(activePath[closestIndex - 1].position, waypoint.position);
        }
        catch (IndexOutOfRangeException)
        {
            distanceBehindClosestPoint = Vector3.Distance(activePath[0].position, waypoint.position);
        }
        try
        {
            distanceInfrontClosestPoint = Vector3.Distance(activePath[closestIndex + 1].position, waypoint.position);
        }
        catch (IndexOutOfRangeException)
        {
            distanceInfrontClosestPoint = Vector3.Distance(activePath[0].position, waypoint.position);
        }

        if (distanceBehindClosestPoint - closestDistance > distanceInfrontClosestPoint - closestDistance)
            newWaypointIndex = closestIndex + 1;
        else
            newWaypointIndex = closestIndex;

        ArrayUtility.Insert(ref activePath, newWaypointIndex, waypoint);
    }

    void Update()
    {
        track.m_Waypoints = activePath;

        currentWaypoint = (int)Mathf.Floor(cart.m_Position);
        if (cart.m_Position >= currentWaypoint + 0.8)
        {
            if (currentWaypoint == activePath.Length - 1)
            {
                activePath = originalPath;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (onAltTrack) CheckIfCartEnd();

            //Should check if the player is actually wanting to move in a direction
            //It is currently just a button you can activate in the inspector
            if(switchingTracks)
            {
                if (!onAltTrack)
                {
                    foreach (CinemachineSmoothPath path in alternativeTracks)
                    {
                        try
                        {
                            if (transform.TransformPoint(activePath[currentWaypoint + 1].position) == path.transform.TransformPoint(path.m_Waypoints[0].position))
                                AAAAAAAAAAAAH(path);
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
            }
        }
    }

    void CheckIfCartEnd()
    {
        if (Mathf.Round(cart.m_Position) == activeAltPath.Length - 1)
        {
            onAltTrack = false;
            activeAltPath = null;
        }
    }

    void AAAAAAAAAAAAH(CinemachineSmoothPath switchingPath1)
    {
        CinemachineSmoothPath switchingPath = switchingPath1;
        onAltTrack = true;
        int firstPointIndex = 0; int endPointIndex = 0;

        for (int i = 0; i < activePath.Length; i++)
        {
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[0].position))
                firstPointIndex = i;
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[switchingPath.m_Waypoints.Length - 1].position))
                endPointIndex = i;
        }

        CinemachineSmoothPath.Waypoint[] temp = new CinemachineSmoothPath.Waypoint[0];

        for (int i = 0; i < activePath.Length; i++)
        {
            if (i > firstPointIndex && i < endPointIndex)
            {
                continue;
            }
            if (i == firstPointIndex)
            {
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
        switchingPath.InvalidateDistanceCache();
    }

}
