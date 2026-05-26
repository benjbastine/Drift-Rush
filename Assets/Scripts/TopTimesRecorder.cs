using UnityEngine;

public class TopTimesRecorder : MonoBehaviour
{
    [Header("Settings")]
    public string mapName = "Map1";

    private int[] recordableLaps = { 1, 5, 10, 15, 20, 25 };
    private LapManager lapManager;
    private bool recorded = false;

    void Start()
    {
        lapManager = GetComponent<LapManager>();
    }

    public void TryRecordTime(float finishTime)
    {
        if (recorded) return;

        int laps = lapManager.totalLaps;

        // Check if this lap count is recordable
        bool isRecordable = false;
        foreach (int lap in recordableLaps)
        {
            if (laps == lap)
            {
                isRecordable = true;
                break;
            }
        }

        if (!isRecordable)
        {
            Debug.Log($"Laps ({laps}) not recordable. No record saved.");
            return;
        }

        string key = $"TopTimes_{mapName}_{laps}Laps";

        // Check if a record already exists
        if (PlayerPrefs.HasKey(key))
        {
            float existingTime = PlayerPrefs.GetFloat(key);

            // Only save if new time is faster
            if (finishTime < existingTime)
            {
                PlayerPrefs.SetFloat(key, finishTime);
                PlayerPrefs.Save();
                Debug.Log($"New best time! {FormatTime(finishTime)} " +
                          $"(Previous: {FormatTime(existingTime)})");
            }
            else
            {
                Debug.Log($"No new record. " +
                          $"Best: {FormatTime(existingTime)}, " +
                          $"This run: {FormatTime(finishTime)}");
            }
        }
        else
        {
            // First time recording
            PlayerPrefs.SetFloat(key, finishTime);
            PlayerPrefs.Save();
            Debug.Log($"First record saved! {FormatTime(finishTime)}");
        }

        recorded = true;
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int millis = (int)((time * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}:{millis:000}";
    }
}