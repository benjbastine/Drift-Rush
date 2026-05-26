using UnityEngine;
using TMPro;

public class TopTimesManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject topTimesMapSelectPanel;
    public GameObject topTimesRecordsPanel;

    [Header("Records Panel")]
    public TextMeshProUGUI recordsTitleText;
    public TextMeshProUGUI recordsText;

    private string selectedMap = "Map1";
    private int[] lapOptions = { 1, 5, 10, 15, 20, 25 };

    public void ShowMapSelect()
    {
        topTimesMapSelectPanel.SetActive(true);
        topTimesRecordsPanel.SetActive(false);
    }

    // ── Map Select ───────────────────────────
    public void OnTTMap1Button()
    {
        selectedMap = "Indy";
        ShowRecords();
    }

    public void OnTTMap2Button()
    {
        selectedMap = "Monaco";
        ShowRecords();
    }

    // ── Records ──────────────────────────────
    void ShowRecords()
    {
        topTimesMapSelectPanel.SetActive(false);
        topTimesRecordsPanel.SetActive(true);

        if (recordsTitleText != null)
            recordsTitleText.text = $"{selectedMap} — Best Time";

        if (recordsText == null) return;

        string display = "";

        foreach (int laps in lapOptions)
        {
            string key = $"TopTimes_{selectedMap}_{laps}Laps";
            string lapLabel = laps == 1 ? "1 Lap" : $"{laps} Laps";

            if (PlayerPrefs.HasKey(key))
            {
                float time = PlayerPrefs.GetFloat(key);
                display += $"{lapLabel}:  {FormatTime(time)}\n\n";
            }
            else
            {
                display += $"{lapLabel}:  No record yet\n\n";
            }
        }

        recordsText.text = display;
    }

    // ── Back ─────────────────────────────────
    public void OnTTRecordsBackButton()
    {
        topTimesMapSelectPanel.SetActive(true);
        topTimesRecordsPanel.SetActive(false);
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        int millis = (int)((time * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}:{millis:000}";
    }
}