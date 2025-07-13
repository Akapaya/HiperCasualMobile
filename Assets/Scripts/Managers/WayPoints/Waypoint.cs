using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
    [Header("Settings")]
    public List<Waypoint> connectedWaypoints = new List<Waypoint>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var point in connectedWaypoints)
        {
            if (point != null)
                Gizmos.DrawLine(transform.position, point.transform.position);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
#endif
}
