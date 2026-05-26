using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("AI Settings")]
    public float maxSpeed = 12f;
    public float accelerationForce = 8f;
    public float steerSpeed = 120f;
    public float friction = 2f;
    public float driftFactor = 0.95f;
    public float waypointReachDistance = 1.5f;

    [Header("Difficulty")]
    [Range(0f, 1f)]
    public float skillLevel = 0.8f;

    // Spin state
    private bool isSpinning = false;
    private float spinTimer = 0f;
    private float spinSpeed = 720f;

    // Boost state
    private bool isBoosting = false;
    private float boostTimer = 0f;
    private float boostForce = 20f;
    private float boostedMaxSpeed;

    // Slow state
    private bool isSlowed = false;
    private float slowTimer = 0f;
    private float slowMultiplier = 0.4f;

    // Shield state
    private bool isShielded = false;
    private float shieldTimer = 0f;
    private ShieldEffect shieldEffect;

    private Rigidbody2D rb;
    private WaypointManager waypointManager;
    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waypointManager = FindAnyObjectByType<WaypointManager>();
        shieldEffect = GetComponentInChildren<ShieldEffect>();
        currentWaypointIndex = Random.Range(0, 2);
        targetWaypoint = waypointManager.GetWaypoint(currentWaypointIndex);
        boostedMaxSpeed = maxSpeed * 2f;
    }

    void Update()
    {
        if (isSpinning)
        {
            spinTimer -= Time.deltaTime;
            if (spinTimer <= 0f) isSpinning = false;
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f) isBoosting = false;
        }

        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f) isSlowed = false;
        }

        if (isShielded)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
            {
                isShielded = false;
                if (shieldEffect != null) shieldEffect.Hide();
                Debug.Log($"{gameObject.name} shield expired!");
            }
        }
    }

    void FixedUpdate()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, spinSpeed * Time.fixedDeltaTime);
            rb.linearVelocity *= 0.98f;
            return;
        }

        if (targetWaypoint == null) return;

        SteerTowardsWaypoint();
        ApplyAcceleration();
        ApplyFriction();
        CapSpeed();
        CheckWaypointReached();
    }

    public void StartShield(float duration)
    {
        isShielded = true;
        shieldTimer = duration;
        if (shieldEffect != null)
        {
            shieldEffect.Show();
            Debug.Log($"{gameObject.name} shield effect shown!");
        }
        else
        {
            Debug.Log($"{gameObject.name} shieldEffect is NULL!");
        }
    }

    public void StartSpin(float duration, float speed)
    {
        if (isShielded)
        {
            Debug.Log($"{gameObject.name} shield blocked oil spill!");
            return;
        }
        isSpinning = true;
        spinTimer = duration;
        spinSpeed = speed;
    }

    public void StartBoost(float duration, float force)
    {
        if (isSpinning) return;
        isBoosting = true;
        boostTimer = duration;
        boostForce = force;
    }

    public void StartSlow(float duration, float multiplier)
    {
        if (isShielded)
        {
            Debug.Log($"{gameObject.name} shield blocked slow!");
            return;
        }
        isSlowed = true;
        slowTimer = duration;
        slowMultiplier = multiplier;
    }

    void SteerTowardsWaypoint()
    {
        Vector2 directionToWaypoint =
            (targetWaypoint.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToWaypoint.x,
                                          directionToWaypoint.y) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;
        float angleDiff = Mathf.DeltaAngle(currentAngle, -targetAngle);
        angleDiff *= skillLevel;

        float steerAmount = Mathf.Clamp(angleDiff, -1f, 1f)
                            * steerSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, 0, steerAmount);
    }

    void ApplyAcceleration()
    {
        Vector2 directionToWaypoint =
            (targetWaypoint.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.up, directionToWaypoint);
        float speedMultiplier = Mathf.Clamp(dot, 0.3f, 1f);

        float currentForce = isSlowed
            ? accelerationForce * slowMultiplier
            : accelerationForce;

        rb.AddForce(transform.up * currentForce * speedMultiplier, ForceMode2D.Force);

        if (isBoosting)
            rb.AddForce(transform.up * boostForce, ForceMode2D.Force);
    }

    void ApplyFriction()
    {
        rb.linearVelocity *= (1f - friction * Time.fixedDeltaTime);

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    void CapSpeed()
    {
        float currentMaxSpeed = isBoosting ? boostedMaxSpeed
                              : isSlowed ? maxSpeed * slowMultiplier
                              : maxSpeed;

        if (rb.linearVelocity.magnitude > currentMaxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
    }

    void CheckWaypointReached()
    {
        float distance = Vector2.Distance(transform.position,
                                          targetWaypoint.position);
        if (distance < waypointReachDistance)
        {
            currentWaypointIndex = waypointManager.GetNextIndex(currentWaypointIndex);
            targetWaypoint = waypointManager.GetWaypoint(currentWaypointIndex);
        }
    }
}