using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndScreenManager : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeShow = 3f;

    private RaceUI raceUI;
    private bool triggered = false;

    void Start()
    {
        raceUI = FindAnyObjectByType<RaceUI>();
    }

    public void TriggerEndScreen()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(ShowEndScreen());
    }

    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(delayBeforeShow);
        if (raceUI != null) raceUI.ShowEndScreen();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}