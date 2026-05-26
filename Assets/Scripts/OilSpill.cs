using UnityEngine;

public class OilSpill : MonoBehaviour
{
    [Header("Oil Spill Settings")]
    public float spinDuration = 1f;
    public float spinSpeed = 720f;
    public float lifetime = 15f;

    [Header("Sound")]
    public AudioClip oilClip;

    private AudioSource audioSource;

    void Start()
    {
        Destroy(gameObject, lifetime);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car"))
        {
            CarController cc = other.GetComponent<CarController>();
            Player2Controller p2 = other.GetComponent<Player2Controller>();
            AICarController ai = other.GetComponent<AICarController>();

            if (cc != null) cc.StartSpin(spinDuration, spinSpeed);
            if (p2 != null) p2.StartSpin(spinDuration, spinSpeed);
            if (ai != null) ai.StartSpin(spinDuration, spinSpeed);

            // Play sound before destroying
            if (oilClip != null)
                AudioSource.PlayClipAtPoint(oilClip, transform.position);

            Destroy(gameObject);
        }
    }
}