using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;

    public Transform GetWaypoint(int index)
    {
        return waypoints[index % waypoints.Length];
    }

    public int GetNextIndex(int index)
    {
        return (index + 1) % waypoints.Length;
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            int next = (i + 1) % waypoints.Length;
            if (waypoints[i] != null && waypoints[next] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
        }
    }
}