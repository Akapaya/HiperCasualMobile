using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Way points to creates paths to NPCs moving without rigid body or navMesh
/// </summary>
[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
    [Header("Settings")]
    public List<Waypoint> connectedWaypoints = new List<Waypoint>();

    /// <summary>
    /// Draw lines to paths to vizualise.
    /// </summary>
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
