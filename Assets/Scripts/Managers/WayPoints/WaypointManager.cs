using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager to handle possible NPC paths, generating random paths according to the NPC's current position.
/// </summary>
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

    #region WayPoints Methods
    /// <summary>
    /// Generate Random Path using random number of waypoints
    /// </summary>
    /// <param name="worldPosition">Current position of NPC</param>
    /// <returns>Return queue of waypoints to move</returns>
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

    /// <summary>
    /// Get the closest waypoint of current position of NPC
    /// </summary>
    /// <param name="worldPosition">Current position of NPC</param>
    /// <returns>Return closest waypoint of NPC</returns>
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
    #endregion
}