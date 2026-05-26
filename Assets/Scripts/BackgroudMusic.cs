using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip musicClip;
    public float volume = 0.5f;
    public float fadeInSpeed = 1f;

    private AudioSource audioSource;
    private bool isFading = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;

        // Start playing immediately when scene loads
        StartMusic();
    }

    public void StartMusic()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.Play();
            isFading = true;
        }
    }

    public void StopMusic()
    {
        isFading = false;
        if (audioSource != null)
            audioSource.Stop();
    }

    void Update()
    {
        if (!isFading) return;

        // Fade in smoothly
        if (audioSource.volume < volume)
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                volume,
                fadeInSpeed * Time.deltaTime
            );
    }
}