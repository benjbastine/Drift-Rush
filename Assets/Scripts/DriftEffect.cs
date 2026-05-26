using UnityEngine;

public class DriftEffect : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem driftMarkEffect;

    [Header("Wheel Positions")]
    public Transform wheelLeft;
    public Transform wheelRight;

    [Header("Drift Settings")]
    public float driftThreshold = 0.5f;
    public float lateralThreshold = 0.1f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsDrifting())
        {
            EmitDriftMarks();
        }
    }

    bool IsDrifting()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed < driftThreshold) return false;

        float lateralSpeed = Mathf.Abs(
            Vector2.Dot(rb.linearVelocity, transform.right)
        );
        return lateralSpeed > lateralThreshold;
    }

    void EmitDriftMarks()
    {
        if (driftMarkEffect == null) return;

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        if (wheelLeft != null)
        {
            emitParams.position = wheelLeft.position;
            driftMarkEffect.Emit(emitParams, 1);
        }

        if (wheelRight != null)
        {
            emitParams.position = wheelRight.position;
            driftMarkEffect.Emit(emitParams, 1);
        }
    }
}