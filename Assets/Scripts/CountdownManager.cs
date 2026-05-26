using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    [Header("Countdown Settings")]
    public float countdownTime = 3f;

    [Header("Countdown Sound")]
    public AudioClip countdownClip;

    private AudioSource audioSource;
    private RaceUI raceUI;
    private BackgroundMusic backgroundMusic;

    void Start()
    {
        raceUI = FindAnyObjectByType<RaceUI>();
        backgroundMusic = FindAnyObjectByType<BackgroundMusic>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void StartCountdownSequence()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        SetAllCarsMovement(false);
        StartAllEngines();


        if (audioSource != null && countdownClip != null)
            audioSource.PlayOneShot(countdownClip);

        int count = (int)countdownTime;
        while (count > 0)
        {
            if (raceUI != null) raceUI.ShowCountdown(count.ToString());
            yield return new WaitForSeconds(1f);
            count--;
        }

        if (raceUI != null) raceUI.ShowCountdown("GO!");
        yield return new WaitForSeconds(0.8f);

        if (raceUI != null) raceUI.HideCountdown();
        SetAllCarsMovement(true);
        RaceManager.Instance.StartRace();
    }

    void StartAllEngines()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in cars)
        {
            EngineSound engine = car.GetComponent<EngineSound>();
            if (engine != null) engine.StartEngine();
        }
    }

    void SetAllCarsMovement(bool canMove)
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject car in cars)
        {
            Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
            CarController cc = car.GetComponent<CarController>();
            Player2Controller p2 = car.GetComponent<Player2Controller>();
            AICarController ai = car.GetComponent<AICarController>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            if (cc != null) cc.canMove = canMove;
            if (p2 != null) p2.canMove = canMove;
            if (ai != null) ai.enabled = canMove;
        }
    }
}