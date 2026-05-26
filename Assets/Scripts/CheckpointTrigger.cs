using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;
    private LapManager lapManager;

    void Start()
    {
        lapManager = FindAnyObjectByType<LapManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger hit by: {other.name} on checkpoint: {checkpointIndex}");

        if (other.CompareTag("Car"))
        {
            Debug.Log($"Car {other.name} hit checkpoint {checkpointIndex}");
            lapManager.CarHitCheckpoint(other.gameObject, checkpointIndex);
        }
    }
}