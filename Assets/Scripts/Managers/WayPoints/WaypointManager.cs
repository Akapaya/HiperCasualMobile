using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointManager : MonoBehaviour
{
    [Header("References")]
    public static WaypointManager Instance;
    public List<Waypoint> allWaypoints = new List<Waypoint>();

    #region Start Methods
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public Queue<Waypoint> GenerateRandomPath(Vector3 worldPosition)
    {
        int numberOfWaypoints = Random.Range(1, allWaypoints.Count -1);
        Queue<Waypoint> waypoints = new ();

        Waypoint current = GetClosestWaypoint(worldPosition);

        for (int i = 0; i < numberOfWaypoints; i++)
        {
            if (current.connectedWaypoints.Count == 0) break;

            Waypoint next = current.connectedWaypoints[Random.Range(0, current.connectedWaypoints.Count)];
            waypoints.Enqueue(next);
            current = next;
        }

        return waypoints;
    }

    public Waypoint GetClosestWaypoint(Vector3 worldPosition)
    {
        Waypoint closest = null;
        float shortestDistance = float.MaxValue;

        foreach (var waypoint in allWaypoints)
        {
            if (waypoint == null) continue;

            float distance = Vector3.Distance(worldPosition, waypoint.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = waypoint;
            }
        }

        return closest;
    }
}