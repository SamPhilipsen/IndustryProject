using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    [SerializeField] Offset offsetScript;
    [SerializeField] private CinemachineDollyCart cart;
    [SerializeField] List<CinemachineSmoothPath> alternativeTracks1;

    private CinemachineSmoothPath track;

    private List<CinemachineSmoothPath.Waypoint> activePath;
    private List<CinemachineSmoothPath.Waypoint> originalPath;
    private List<CinemachineSmoothPath.Waypoint> activeAltPath;
    private List<CinemachineSmoothPath> alternativeTracks;

    [NonSerialized]
    public string switchingTracks;
    private bool onAltTrack;
    private int currentWaypoint;

    public delegate void NearingSwitch(bool nearingSwitch);
    public NearingSwitch nearingSwitch;

    void Start()
    {
        track = GetComponentInParent<CinemachineSmoothPath>();
        activePath = new List<CinemachineSmoothPath.Waypoint>();
        originalPath = new List<CinemachineSmoothPath.Waypoint>();
        activeAltPath = new List<CinemachineSmoothPath.Waypoint>();
        alternativeTracks = new List<CinemachineSmoothPath>();

        activePath.AddRange(track.m_Waypoints);
        onAltTrack = false;
        currentWaypoint = 0;
        switchingTracks = "forward";

        foreach (CinemachineSmoothPath path in alternativeTracks1)
        {
            alternativeTracks.Add(path);
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
        for (int i = 0; i < activePath.Count; i++)
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

        activePath.Insert(newWaypointIndex, waypoint);
    }

    void Update()
    {
        track.m_Waypoints = activePath.ToArray();

        currentWaypoint = (int)Mathf.Floor(cart.m_Position);
        if (cart.m_Position >= currentWaypoint + 0.8)
        {
            if (currentWaypoint == activePath.Count - 1)
            {
                activePath = originalPath;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (onAltTrack)
            {
                CheckIfCartEnd();
                nearingSwitch(false);
            }


            if (!onAltTrack)
            {
                foreach (CinemachineSmoothPath path in alternativeTracks)
                {
                    try
                    {
                        if (currentWaypoint + 1 == 1 || currentWaypoint + 1 == 2 || currentWaypoint + 1== 4 || currentWaypoint + 1== 5)
                            nearingSwitch(true);
                        else
                            nearingSwitch(false);

                        if (transform.TransformPoint(activePath[currentWaypoint + 1].position) == path.transform.TransformPoint(path.m_Waypoints[0].position))
                        {
                            if(path.GetComponent<TrackSideController>().trackSide == "left" && switchingTracks == "left")
                                AAAAAAAAAAAAH(path);
                            if(path.GetComponent<TrackSideController>().trackSide == "right" && switchingTracks == "right")
                                AAAAAAAAAAAAH(path);
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
        }
    }

    void CheckIfCartEnd()
    {
        if (Mathf.Round(cart.m_Position) == activeAltPath.Count - 1)
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

        for (int i = 0; i < activePath.Count; i++)
        {
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[0].position))
                firstPointIndex = i;
            if (transform.TransformPoint(activePath[i].position) == switchingPath.transform.TransformPoint(switchingPath.m_Waypoints[switchingPath.m_Waypoints.Length - 1].position))
                endPointIndex = i;
        }

        List<CinemachineSmoothPath.Waypoint> temp = new List<CinemachineSmoothPath.Waypoint>();

        for (int i = 0; i < activePath.Count; i++)
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
                temp.AddRange(switchingPath.m_Waypoints);
            }
            if (i != firstPointIndex && i != endPointIndex) temp.Add(activePath[i]);
        }
        activeAltPath.AddRange(switchingPath.m_Waypoints);
        activePath = temp;
        switchingPath.InvalidateDistanceCache();
    }

}