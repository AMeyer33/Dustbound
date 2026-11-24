using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CowboyPlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("2.5D Jump Settings")]
    public float jumpDuration = 0.45f;
    public float jumpScaleMultiplier = 1.25f;     // Peak height scale while mid-air
    public float elevatedScaleMultiplier = 1.12f; // Subtle scale boost while standing on top of an object
    public LayerMask jumpableLayer;               // Select ONLY "Jumpable" in the Inspector

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 moveInput;
    private Vector3 baseScale;
    private float jumpTimer = 0f;
    private bool isJumping = false;
    private Collider2D standingOnObject = null; // Tracks the object you landed on

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        baseScale = transform.localScale;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Read WASD input
        float x = 0f;
        float y = 0f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed) y -= 1f;
        if (Keyboard.current.wKey.isPressed) y += 1f;

        moveInput = new Vector2(x, y).normalized;

        if (spriteRenderer != null && x != 0f)
        {
            spriteRenderer.flipX = x < 0f;
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", moveInput.sqrMagnitude > 0.01f);
        }

        // 2. Start jump when Spacebar is pressed (works from ground OR while standing on an object)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isJumping)
        {
            StartJump();
        }

        // 3. Handle mid-air jump arc
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float progress = jumpTimer / jumpDuration;

            if (progress >= 1f)
            {
                EndJump();
            }
            else
            {
                float arc = Mathf.Sin(progress * Mathf.PI);
                float startScale = (standingOnObject != null) ? elevatedScaleMultiplier : 1f;
                float currentScale = Mathf.Lerp(startScale, jumpScaleMultiplier, arc);
                transform.localScale = baseScale * currentScale;
            }
        }
        // 4. Check if the player walked off the edge of the object they landed on
        else if (standingOnObject != null)
        {
            // Check if player collider is still touching the object's bounds
            if (!playerCollider.bounds.Intersects(standingOnObject.bounds))
            {
                StepOffObject();
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        SetJumpableCollisions(ignore: true);
    }

    void EndJump()
    {
        isJumping = false;

        // Check if we landed on top of a Jumpable object's collider
        Collider2D hit = Physics2D.OverlapBox(
            playerCollider.bounds.center,
            playerCollider.bounds.size * 0.8f,
            0f,
            jumpableLayer
        );

        if (hit != null)
        {
            // Stay on top of this object! Keep its collision ignored and keep a slight elevated scale
            standingOnObject = hit;
            transform.localScale = baseScale * elevatedScaleMultiplier;
        }
        else
        {
            // Landed back on the sand floor
            standingOnObject = null;
            transform.localScale = baseScale;
            SetJumpableCollisions(ignore: false);
        }
    }

    void StepOffObject()
    {
        standingOnObject = null;
        transform.localScale = baseScale;
        SetJumpableCollisions(ignore: false);
    }

    void SetJumpableCollisions(bool ignore)
    {
        int playerLayer = gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if ((jumpableLayer.value & (1 << i)) != 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, i, ignore);
            }
        }
    }
}