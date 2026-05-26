using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartScreenManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject startScreenPanel;
    public TextMeshProUGUI mapInfoText;

    [Header("Map Info")]
    public string mapName = "Map1"; // Change per scene

    private CountdownManager countdownManager;

    void Start()
    {
        countdownManager = GetComponent<CountdownManager>();

        // Show start screen first
        if (startScreenPanel != null)
            startScreenPanel.SetActive(true);

        // Show map and lap info
        UpdateMapInfo();
    }

    void UpdateMapInfo()
    {
        if (mapInfoText == null) return;

        int laps = PlayerPrefs.GetInt("SelectedLaps", 1);
        int mode = PlayerPrefs.GetInt("PlayerMode", 1);
        string mode_text = mode == 2 ? "2 Player" : "Single Player";

        mapInfoText.text = $"Map: {mapName}\n" +
                           $"Laps: {laps}\n" +
                           $"Mode: {mode_text}";
    }

    public void OnStartButton()
    {
        // Hide start screen
        if (startScreenPanel != null)
            startScreenPanel.SetActive(false);

        // Begin countdown
        if (countdownManager != null)
            countdownManager.StartCountdownSequence();
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}