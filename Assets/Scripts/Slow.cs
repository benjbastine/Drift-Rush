using UnityEngine;

public class Slow : MonoBehaviour
{
    [Header("Slow Settings")]
    public float slowDuration = 3f;
    public float slowMultiplier = 0.4f;
    public float respawnTime = 5f;

    [Header("Sound")]
    public AudioClip slowClip;

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

            if (cc != null) cc.StartSlow(slowDuration, slowMultiplier);
            if (p2 != null) p2.StartSlow(slowDuration, slowMultiplier);
            if (ai != null) ai.StartSlow(slowDuration, slowMultiplier);

            // Play sound
            if (audioSource != null && slowClip != null)
                audioSource.PlayOneShot(slowClip);

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