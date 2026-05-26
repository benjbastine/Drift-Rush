using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject modeSelectPanel;
    public GameObject mapSelectPanel;
    public GameObject topTimesPanel;

    [Header("Lap Selector")]
    public TextMeshProUGUI lapCountText;
    public int defaultLaps = 1;
    public int minLaps = 1;
    public int maxLaps = 25;

    private int selectedLaps;

    void Start()
    {
        selectedLaps = defaultLaps;
        UpdateLapDisplay();
        ShowPanel(mainPanel);
    }

    // ── Main Panel ──────────────────────────
    public void OnPlayButton()
    {
        ShowPanel(modeSelectPanel);
    }

    public void OnTopTimesButton()
    {
        ShowPanel(topTimesPanel);
        FindAnyObjectByType<TopTimesManager>().ShowMapSelect();
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ── Mode Select Panel ───────────────────
    public void OnSinglePlayerButton()
    {
        PlayerPrefs.SetInt("PlayerMode", 1);
        ShowPanel(mapSelectPanel);
    }

    public void OnTwoPlayerButton()
    {
        PlayerPrefs.SetInt("PlayerMode", 2);
        ShowPanel(mapSelectPanel);
    }

    public void OnBackToMainButton()
    {
        ShowPanel(mainPanel);
    }

    // ── Lap Selector ────────────────────────
    public void OnIncreaseLap()
    {
        if (selectedLaps < maxLaps)
        {
            selectedLaps++;
            UpdateLapDisplay();
        }
    }

    public void OnDecreaseLap()
    {
        if (selectedLaps > minLaps)
        {
            selectedLaps--;
            UpdateLapDisplay();
        }
    }

    void UpdateLapDisplay()
    {
        if (lapCountText != null)
            lapCountText.text = $"{selectedLaps}";
    }

    // ── Map Select Panel ────────────────────
    public void OnMap1Button()
    {
        int mode = PlayerPrefs.GetInt("PlayerMode", 1);
        SaveLapsAndLoad(mode == 2 ? "Indy2P" : "Indy");
    }

    public void OnMap2Button()
    {
        int mode = PlayerPrefs.GetInt("PlayerMode", 1);
        SaveLapsAndLoad(mode == 2 ? "Monaco2P" : "Monaco");
    }

    public void OnBackToModeButton()
    {
        ShowPanel(modeSelectPanel);
    }

    // ── Top Times Panel ─────────────────────
    public void OnBackFromTopTimesButton()
    {
        ShowPanel(mainPanel);
    }

    // ── Helper ──────────────────────────────
    void SaveLapsAndLoad(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedLaps", selectedLaps);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneName);
    }

    void ShowPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        modeSelectPanel.SetActive(false);
        mapSelectPanel.SetActive(false);
        topTimesPanel.SetActive(false);
        panel.SetActive(true);
    }
}