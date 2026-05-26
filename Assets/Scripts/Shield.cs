using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Shield Settings")]
    public float shieldDuration = 5f;
    public float lifetime = 20f;

    [Header("Sound")]
    public AudioClip shieldClip;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car"))
        {
            CarController cc = other.GetComponent<CarController>();
            Player2Controller p2 = other.GetComponent<Player2Controller>();
            AICarController ai = other.GetComponent<AICarController>();

            Debug.Log($"{other.name} hit the shield!");

            if (cc != null) { cc.StartShield(shieldDuration); Debug.Log("Player1 shield activated!"); }
            if (p2 != null) { p2.StartShield(shieldDuration); Debug.Log("Player2 shield activated!"); }
            if (ai != null) { ai.StartShield(shieldDuration); Debug.Log($"{other.name} AI shield activated!"); }

            if (shieldClip != null)
                AudioSource.PlayClipAtPoint(shieldClip, transform.position);

            Destroy(gameObject);
        }
    }
}