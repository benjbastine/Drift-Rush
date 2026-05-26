using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    [Header("Shield Spawner Settings")]
    public GameObject shieldPrefab;
    public float spawnInterval = 12f;
    public int maxShields = 3;

    private WaypointManager waypointManager;
    private int currentShieldCount = 0;
    private bool raceStarted = false;

    void Start()
    {
        waypointManager = FindAnyObjectByType<WaypointManager>();
    }

    void Update()
    {
        // Wait until race starts then begin spawning
        if (!raceStarted && RaceManager.Instance != null
            && RaceManager.Instance.IsRaceStarted())
        {
            raceStarted = true;
            InvokeRepeating(nameof(SpawnShield), 5f, spawnInterval);
        }
    }

    void SpawnShield()
    {
        if (currentShieldCount >= maxShields) return;
        if (waypointManager == null) return;

        int randomIndex = Random.Range(0, waypointManager.waypoints.Length);
        Transform spawnPoint = waypointManager.waypoints[randomIndex];

        Vector2 offset = Random.insideUnitCircle * 0.8f;
        Vector3 spawnPos = spawnPoint.position +
                           new Vector3(offset.x, offset.y, 0);

        GameObject shield = Instantiate(shieldPrefab, spawnPos,
                                          Quaternion.identity);
        currentShieldCount++;

        Shield shieldScript = shield.GetComponent<Shield>();
        if (shieldScript != null)
            StartCoroutine(TrackShieldLifetime(shieldScript.lifetime));
    }

    System.Collections.IEnumerator TrackShieldLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        currentShieldCount = Mathf.Max(0, currentShieldCount - 1);
    }
}