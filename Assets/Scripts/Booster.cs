using UnityEngine;

public class Booster : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostForce = 20f;
    public float boostDuration = 2f;
    public float respawnTime = 5f;

    [Header("Sound")]
    public AudioClip boostClip;

    private bool isActive = true;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Car"))
        {
            CarController cc = other.GetComponent<CarController>();
            Player2Controller p2 = other.GetComponent<Player2Controller>();
            AICarController ai = other.GetComponent<AICarController>();

            if (cc != null) cc.StartBoost(boostDuration, boostForce);
            if (p2 != null) p2.StartBoost(boostDuration, boostForce);
            if (ai != null) ai.StartBoost(boostDuration, boostForce);

            // Play sound
            if (audioSource != null && boostClip != null)
                audioSource.PlayOneShot(boostClip);

            StartCoroutine(Respawn());
        }
    }

    System.Collections.IEnumerator Respawn()
    {
        isActive = false;
        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        sr.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        isActive = true;
    }
}