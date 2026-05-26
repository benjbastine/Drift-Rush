using UnityEngine;
using System.Collections.Generic;

public class LapManager : MonoBehaviour
{
    [Header("Settings")]
    public int totalLaps = 1;
    public CheckpointTrigger[] checkpoints;

    private Dictionary<GameObject, int> nextCheckpoint =
        new Dictionary<GameObject, int>();
    private Dictionary<GameObject, int> lapCount =
        new Dictionary<GameObject, int>();
    private Dictionary<GameObject, bool> finished =
        new Dictionary<GameObject, bool>();

    private RaceUI raceUI;

    void Awake()
    {
        // Read selected laps from MainMenu
        if (PlayerPrefs.HasKey("SelectedLaps"))
            totalLaps = PlayerPrefs.GetInt("SelectedLaps");

        Debug.Log($"Total Laps set to: {totalLaps}");
    }

    void Start()
    {
        raceUI = FindAnyObjectByType<RaceUI>();
    }

    public void RegisterCar(GameObject car)
    {
        nextCheckpoint[car] = 0;
        lapCount[car] = 0;
        finished[car] = false;
    }

    public void CarHitCheckpoint(GameObject car, int index)
    {
        if (!nextCheckpoint.ContainsKey(car)) return;
        if (finished[car]) return;
        if (index != nextCheckpoint[car]) return;

        nextCheckpoint[car]++;

        if (nextCheckpoint[car] >= checkpoints.Length)
        {
            nextCheckpoint[car] = 0;
            lapCount[car]++;

            Debug.Log($"{car.name} completed lap {lapCount[car]}!");

            RaceManager.Instance.CarCompletedLap(car.name, lapCount[car]);

            // Update UI for Player1 and Player2
            if (car.name == "Player 1" && raceUI != null)
                raceUI.UpdateLapDisplay(lapCount[car] + 1);

            if (car.name == "Player 2" && raceUI != null)
                raceUI.UpdatePlayer2LapDisplay(lapCount[car] + 1);

            if (lapCount[car] >= totalLaps)
            {
                finished[car] = true;
                RaceManager.Instance.CarFinishedRace(car.name);
                Debug.Log($"{car.name} FINISHED THE RACE!");
            }
        }
    }

    public int GetLap(GameObject car)
    {
        return lapCount.ContainsKey(car) ? lapCount[car] : 0;
    }
}