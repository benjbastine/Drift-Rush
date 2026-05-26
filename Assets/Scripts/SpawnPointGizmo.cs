using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawLine(
            transform.position,
            transform.position + transform.up * 1.5f
        );
    }
}