using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float accelerationForce = 8f;
    public float maxSpeed = 12f;
    public float steerSpeed = 120f;
    public float friction = 2f;
    public float driftFactor = 0.95f;

    [HideInInspector]
    public bool canMove = false; // Controlled by CountdownManager

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
    private float moveInput;
    private float steerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boostedMaxSpeed = maxSpeed * 2f;
        shieldEffect = GetComponentInChildren<ShieldEffect>();
    }

    void Update()
    {
        // Block everything if cant move
        if (!canMove)
        {
            moveInput = 0f;
            steerInput = 0f;
            return;
        }

        // Spin timer
        if (isSpinning)
        {
            spinTimer -= Time.deltaTime;
            if (spinTimer <= 0f) isSpinning = false;
            return;
        }

        // Boost timer
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f) isBoosting = false;
        }

        // Slow timer
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f) isSlowed = false;
        }

        // Shield timer
        if (isShielded)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
            {
                isShielded = false;
                if (shieldEffect != null) shieldEffect.Hide();
            }
        }

        // Player 1 uses Arrow Keys only
        Vector2 input = new Vector2(
            (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) -
            (Keyboard.current.leftArrowKey.isPressed ? 1 : 0),
            (Keyboard.current.upArrowKey.isPressed ? 1 : 0) -
            (Keyboard.current.downArrowKey.isPressed ? 1 : 0)
        );

        steerInput = input.x;
        moveInput = input.y;
    }

    void FixedUpdate()
    {
        // Hard freeze during countdown
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        if (isSpinning)
        {
            transform.Rotate(0, 0, spinSpeed * Time.fixedDeltaTime);
            rb.linearVelocity *= 0.98f;
            return;
        }

        ApplyAcceleration();
        ApplySteering();
        ApplyFriction();
        CapSpeed();
    }

    public void StartShield(float duration)
    {
        isShielded = true;
        shieldTimer = duration;
        if (shieldEffect != null) shieldEffect.Show();
    }

    public void StartSpin(float duration, float speed)
    {
        if (isShielded) return;
        isSpinning = true;
        spinTimer = duration;
        spinSpeed = speed;
        moveInput = 0f;
        steerInput = 0f;
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
        if (isShielded) return;
        isSlowed = true;
        slowTimer = duration;
        slowMultiplier = multiplier;
    }

    void ApplyAcceleration()
    {
        float currentForce = isSlowed
            ? accelerationForce * slowMultiplier
            : accelerationForce;

        rb.AddForce(transform.up * moveInput * currentForce, ForceMode2D.Force);

        if (isBoosting)
            rb.AddForce(transform.up * boostForce, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed < 0.5f) return;

        float steerAmount = steerInput * steerSpeed * Time.fixedDeltaTime;
        if (moveInput < 0) steerAmount = -steerAmount;
        transform.Rotate(0, 0, -steerAmount);
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
}