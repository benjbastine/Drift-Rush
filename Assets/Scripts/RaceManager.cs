using UnityEngine;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    [Header("Race Info")]
    public int totalCars = 4;

    [Header("2 Player Mode")]
    public bool isTwoPlayerMode = false;

    private float raceStartTime;
    private bool raceStarted = false;
    private bool raceFinished = false;

    private bool player1Finished = false;
    private bool player2Finished = false;

    private List<string> finishingOrder = new List<string>();
    private Dictionary<string, float> finishTimes =
        new Dictionary<string, float>();

    private EndScreenManager endScreenManager;
    private TopTimesRecorder topTimesRecorder;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        endScreenManager = FindAnyObjectByType<EndScreenManager>();
        topTimesRecorder = FindAnyObjectByType<TopTimesRecorder>();

        if (PlayerPrefs.HasKey("PlayerMode"))
            isTwoPlayerMode = PlayerPrefs.GetInt("PlayerMode") == 2;
    }

    public void StartRace()
    {
        raceStartTime = Time.time;
        raceStarted = true;
        Debug.Log("Race Started!");
    }

    public bool IsRaceStarted()
    {
        return raceStarted;
    }

    public void CarCompletedLap(string carName, int lap)
    {
        if (!raceStarted) return;
        Debug.Log($"{carName} completed lap {lap}!");
    }

    public void CarFinishedRace(string carName)
    {
        if (finishingOrder.Contains(carName)) return;

        finishingOrder.Add(carName);
        float totalTime = Time.time - raceStartTime;
        finishTimes[carName] = totalTime;

        string position = GetPositionLabel(finishingOrder.Count);
        Debug.Log($"{carName} finished {position}! " +
                  $"Total Time: {FormatTime(totalTime)}");

        if (carName == "Player 1")
        {
            player1Finished = true;

            // Record time for single player only
            if (!isTwoPlayerMode && topTimesRecorder != null)
                topTimesRecorder.TryRecordTime(totalTime);
        }

        if (carName == "Player 2") player2Finished = true;

        // Check if race should end
        if (isTwoPlayerMode)
        {
            if (player1Finished && player2Finished)
            {
                raceFinished = true;
                if (endScreenManager != null)
                    endScreenManager.TriggerEndScreen();
            }
        }
        else
        {
            if (player1Finished)
            {
                raceFinished = true;
                if (endScreenManager != null)
                    endScreenManager.TriggerEndScreen();
            }
        }
    }

    public float GetRaceTime()
    {
        return raceStarted ? Time.time - raceStartTime : 0f;
    }

    public List<string> GetFinishingOrder()
    {
        return finishingOrder;
    }

    public string GetFinishTime(string carName)
    {
        return finishTimes.ContainsKey(carName)
            ? FormatTime(finishTimes[carName])
            : "--:--:---";
    }

    public string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int millis = (int)((time * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}:{millis:000}";
    }

    string GetPositionLabel(int position)
    {
        switch (position)
        {
            case 1: return "1st";
            case 2: return "2nd";
            case 3: return "3rd";
            default: return $"{position}th";
        }
    }
}