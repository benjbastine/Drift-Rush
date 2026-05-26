using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RaceUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI raceTimerText;
    public TextMeshProUGUI currentLapText;
    public TextMeshProUGUI player2LapText; // NEW
    public TextMeshProUGUI countdownText;

    [Header("End Screen")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI endScreenResultsText;
    public Button playAgainButton;
    public Button backButton;

    [Header("Player Car")]
    public string playerCarName = "Player 1";
    public string player2CarName = "Player 2";

    private RaceManager raceManager;
    private LapManager lapManager;
    private EndScreenManager endScreenManager;

    void Start()
    {
        raceManager = FindAnyObjectByType<RaceManager>();
        lapManager = FindAnyObjectByType<LapManager>();
        endScreenManager = FindAnyObjectByType<EndScreenManager>();

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        if (endScreenPanel != null)
            endScreenPanel.SetActive(false);

        // Hide Player2 lap text if single player
        if (player2LapText != null)
        {
            bool isTwoPlayer = PlayerPrefs.GetInt("PlayerMode", 1) == 2;
            player2LapText.gameObject.SetActive(isTwoPlayer);
        }

        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(endScreenManager.PlayAgain);

        if (backButton != null)
            backButton.onClick.AddListener(endScreenManager.BackToMainMenu);

        UpdateLapDisplay(1);
        UpdatePlayer2LapDisplay(1);
    }

    void Update()
    {
        UpdateTimer();
        UpdateEndScreenResults();
    }

    public void UpdateLapDisplay(int currentLap)
    {
        if (currentLapText == null) return;
        int total = lapManager != null ? lapManager.totalLaps : 1;
        currentLap = Mathf.Min(currentLap, total);
        currentLapText.text = $"P1 Lap {currentLap} / {total}";
    }

    public void UpdatePlayer2LapDisplay(int currentLap)
    {
        if (player2LapText == null) return;
        int total = lapManager != null ? lapManager.totalLaps : 1;
        currentLap = Mathf.Min(currentLap, total);
        player2LapText.text = $"P2 Lap {currentLap} / {total}";
    }

    public void ShowEndScreen()
    {
        if (endScreenPanel == null) return;
        endScreenPanel.SetActive(true);
    }

    void UpdateEndScreenResults()
    {
        if (endScreenPanel == null) return;
        if (!endScreenPanel.activeSelf) return;
        if (endScreenResultsText == null) return;

        List<string> order = raceManager.GetFinishingOrder();
        string[] labels = { "1st", "2nd", "3rd", "4th" };
        string display = "Final Results\n\n";

        for (int i = 0; i < order.Count; i++)
        {
            string time = raceManager.GetFinishTime(order[i]);
            display += $"{labels[i]}:  {order[i]}  -  {time}\n";
        }

        for (int i = order.Count; i < raceManager.totalCars; i++)
            display += $"{labels[i]}:  --:--:---\n";

        endScreenResultsText.text = display;
    }

    public void ShowCountdown(string text)
    {
        if (countdownText == null) return;
        countdownText.gameObject.SetActive(true);
        countdownText.text = text;
    }

    public void HideCountdown()
    {
        if (countdownText == null) return;
        countdownText.gameObject.SetActive(false);
    }

    void UpdateTimer()
    {
        if (raceTimerText == null) return;

        if (!raceManager.IsRaceStarted())
        {
            raceTimerText.text = "Time: 00:00:000";
            return;
        }

        raceTimerText.text = "Time: " +
            raceManager.FormatTime(raceManager.GetRaceTime());
    }
}