using UnityEngine;

public class OilSpillSpawner : MonoBehaviour
{
    [Header("Oil Spill Settings")]
    public GameObject oilSpillPrefab;
    public float spawnInterval = 8f;
    public int maxOilSpills = 5;
    public float oilSpillScale = 1.5f;

    private WaypointManager waypointManager;
    private int currentOilCount = 0;
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
            InvokeRepeating(nameof(SpawnOilSpill), 3f, spawnInterval);
        }
    }

    void SpawnOilSpill()
    {
        if (currentOilCount >= maxOilSpills) return;
        if (waypointManager == null) return;

        int randomIndex = Random.Range(0, waypointManager.waypoints.Length);
        Transform spawnPoint = waypointManager.waypoints[randomIndex];

        Vector2 offset = Random.insideUnitCircle * 0.8f;
        Vector3 spawnPos = spawnPoint.position +
                           new Vector3(offset.x, offset.y, 0);

        GameObject oil = Instantiate(oilSpillPrefab, spawnPos,
                                       Quaternion.identity);
        oil.transform.localScale = Vector3.one * oilSpillScale;
        currentOilCount++;

        OilSpill oilScript = oil.GetComponent<OilSpill>();
        if (oilScript != null)
            StartCoroutine(TrackOilLifetime(oilScript.lifetime));
    }

    System.Collections.IEnumerator TrackOilLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        currentOilCount = Mathf.Max(0, currentOilCount - 1);
    }
}