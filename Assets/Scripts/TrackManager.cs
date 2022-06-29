using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart cart;
    [SerializeField] List<CinemachineSmoothPath> alternativeTracks1;

    private CinemachineSmoothPath track;

    private List<CinemachineSmoothPath.Waypoint> activePath;
    private List<CinemachineSmoothPath.Waypoint> originalPath;
    private List<CinemachineSmoothPath> alternativeTracks;
    Dictionary<int, CinemachineSmoothPath> pathCollisionPoints;
    private int endOfAltTrackWaypoint;

    [NonSerialized]
    public string switchingTracks;
    private bool onAltTrack;
    private int currentWaypoint;

    public delegate void NearingSwitch(CinemachineSmoothPath path);
    public NearingSwitch nearingSwitch;

    void Start()
    {
        track = GetComponentInParent<CinemachineSmoothPath>();
        activePath = new List<CinemachineSmoothPath.Waypoint>();
        originalPath = new List<CinemachineSmoothPath.Waypoint>();
        alternativeTracks = new List<CinemachineSmoothPath>(alternativeTracks1);
        pathCollisionPoints = new Dictionary<int, CinemachineSmoothPath>();        

        activePath.AddRange(track.m_Waypoints);
        onAltTrack = false;
        currentWaypoint = 0;
        endOfAltTrackWaypoint = 0;
        switchingTracks = "forward";

        foreach (CinemachineSmoothPath path in alternativeTracks)
        {
            Vector3 firstWaypointPos, lastWaypointPos;
            firstWaypointPos = path.transform.TransformPoint(path.m_Waypoints[0].position);
            lastWaypointPos = path.transform.TransformPoint(path.m_Waypoints[path.m_Waypoints.Length - 1].position);

            PlaceWaypointOnTrack(firstWaypointPos);
            PlaceWaypointOnTrack(lastWaypointPos);
        }
        FindCollisionPoints();
        originalPath = activePath;
    }
    void FindCollisionPoints()
    {
        pathCollisionPoints = new Dictionary<int, CinemachineSmoothPath>();
        foreach (CinemachineSmoothPath path in alternativeTracks)
        {
            int collisionPoint = 0;
            Vector3 waypointPos = track.transform.InverseTransformPoint(path.transform.TransformPoint(path.m_Waypoints[0].position));

            for (int i = 0; i < activePath.Count; i++)
            {
                if (activePath[i].position == waypointPos) collisionPoint = i;
            }
            pathCollisionPoints.Add(collisionPoint, path);
        }
    }
    void PlaceWaypointOnTrack(Vector3 waypointPos)
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

        float distanceBehindClosestPoint = 0;
        float distanceInfrontClosestPoint = 0; 

        try
        {
            distanceBehindClosestPoint = Vector3.Distance(activePath[closestIndex - 1].position, waypoint.position);
        }
        catch (Exception ex)
        {
            if(ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
                distanceBehindClosestPoint = Vector3.Distance(activePath[0].position, waypoint.position);
        }
        try
        {
            distanceInfrontClosestPoint = Vector3.Distance(activePath[closestIndex + 1].position, waypoint.position);
        }
        catch (Exception ex)
        {
            if (ex is IndexOutOfRangeException || ex is ArgumentOutOfRangeException)
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

        if (currentWaypoint == 0)
        {
            activePath = originalPath;
        }

        if (cart.m_Position >= currentWaypoint + 0.95)
        {     
            if (onAltTrack)
            {
                CheckIfCartEnd();
            }

            if (!onAltTrack)
            {
                CheckSwitchingStatus();
            }
        }
    }

    void CheckSwitchingStatus()
    {
        foreach (KeyValuePair<int, CinemachineSmoothPath> collisionPoint in pathCollisionPoints)
        {
            if (currentWaypoint == collisionPoint.Key - 2)
                nearingSwitch(collisionPoint.Value);

            if(currentWaypoint + 1 == collisionPoint.Key)
            {
                nearingSwitch(null);
                CinemachineSmoothPath path = collisionPoint.Value;
                if (path.GetComponent<TrackSideController>().trackSide == switchingTracks)
                    SwitchToDifferentTrack(path);
            }
        }
    }

    void SwitchToDifferentTrack(CinemachineSmoothPath switchingPath1)
    {
        GameObject switchingPath = Instantiate(switchingPath1.gameObject);
        CinemachineSmoothPath.Waypoint[] waypoints = switchingPath.GetComponent<CinemachineSmoothPath>().m_Waypoints;

        onAltTrack = true;
        int firstPointIndex = 0; int endPointIndex = 0;

        for (int i = 0; i < activePath.Count; i++)
        {
            if (transform.TransformPoint(activePath[i].position) == switchingPath1.transform.TransformPoint(waypoints[0].position))
                firstPointIndex = i;
            if (transform.TransformPoint(activePath[i].position) == switchingPath1.transform.TransformPoint(waypoints[waypoints.Length - 1].position))
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
                for (int x = 0; x < waypoints.Length; x++)
                {
                    waypoints[x].position = switchingPath1.transform.TransformPoint(waypoints[x].position);
                    waypoints[x].position = track.transform.InverseTransformPoint(waypoints[x].position);
                }
                temp.AddRange(waypoints);
            }
            if (i != firstPointIndex && i != endPointIndex) temp.Add(activePath[i]);
        }
        endOfAltTrackWaypoint = currentWaypoint + waypoints.Length;
        Destroy(switchingPath);
        activePath = temp;
        FindCollisionPoints();
    }

    void CheckIfCartEnd()
    {
        if (currentWaypoint == endOfAltTrackWaypoint)
            onAltTrack = false;
    }

}
