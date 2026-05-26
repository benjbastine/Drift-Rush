using UnityEngine;

public class EngineSound : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip engineClip;

    [Header("Pitch Settings")]
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float pitchSmooth = 2f;

    [Header("Volume Settings")]
    public float minVolume = 0.3f;
    public float maxVolume = 0.8f;

    [Header("AI Settings")]
    public bool isAI = false;

    private AudioSource engineSource;
    private Rigidbody2D rb;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (isAI)
        {
            minVolume = 0.05f;
            maxVolume = 0.15f;
        }

        // Set up Audio Source but don't play yet
        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.clip = engineClip;
        engineSource.loop = true;
        engineSource.playOnAwake = false;
        engineSource.pitch = minPitch;
        engineSource.volume = minVolume;
    }

    void Update()
    {
        if (!isRunning || rb == null || engineSource == null) return;

        float speed = rb.linearVelocity.magnitude;
        float maxSpeed = GetMaxSpeed();
        float speedRatio = Mathf.Clamp01(speed / maxSpeed);

        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, speedRatio);

        engineSource.pitch = Mathf.Lerp(engineSource.pitch,
                                          targetPitch,
                                          pitchSmooth * Time.deltaTime);
        engineSource.volume = Mathf.Lerp(engineSource.volume,
                                          targetVolume,
                                          pitchSmooth * Time.deltaTime);
    }

    // Called by CountdownManager when countdown starts
    public void StartEngine()
    {
        if (engineSource != null && !engineSource.isPlaying)
        {
            isRunning = true;
            engineSource.Play();
        }
    }

    public void StopEngine()
    {
        isRunning = false;
        if (engineSource != null)
            engineSource.Stop();
    }

    float GetMaxSpeed()
    {
        CarController cc = GetComponent<CarController>();
        Player2Controller p2 = GetComponent<Player2Controller>();
        AICarController ai = GetComponent<AICarController>();

        if (cc != null) return cc.maxSpeed;
        if (p2 != null) return p2.maxSpeed;
        if (ai != null) return ai.maxSpeed;

        return 12f;
    }
}