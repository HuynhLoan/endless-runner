using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 8f;
    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Air Jump Energy")]
    public int maxAirEnergy = 1;
    private int airEnergy;

    [Header("Health")]
    public int maxHealth = 3;
    public Slider healthBar;
    private int currentHealth;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public GameObject coinCollectEffectPrefab;

    [Header("Damage Settings")]
    public float invincibleTime = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Camera cam;

    private bool isGrounded;
    private bool isDead = false;
    private bool isPaused = false;
    private bool canTakeDamage = true;

    // out camera
    private bool outCameraTriggered = false;
    private Vector3 respawnPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        Time.timeScale = 1f;
        cam = Camera.main;

        Vector3 leftScreen = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f));
        transform.position = new Vector3(leftScreen.x + 2f, transform.position.y, 0f);

        respawnPosition = transform.position;

        airEnergy = maxAirEnergy;

        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        if (isDead || isPaused) return;

        // ===== GROUND CHECK =====
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded)
        {
            airEnergy = maxAirEnergy;
            anim.SetBool("isJumping", false);
        }

        // ===== JUMP =====
        bool jumpInput =
            Input.GetButtonDown("Jump") ||     // Space
            Input.GetMouseButtonDown(0);       // Chuột trái

        if (jumpInput)
        {
            if (isGrounded)
                Jump();
            else if (airEnergy > 0)
            {
                Jump();
                airEnergy--;
            }
        }


        // ===== BETTER FALL =====
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y *
                (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y *
                (lowJumpMultiplier - 1f) * Time.deltaTime;
        }

        CheckOutOfCamera();
    }

    void LateUpdate()
    {
        if (isDead) return;

        Renderer rend = GetComponent<Renderer>();
        if (!rend) return;

        Bounds b = rend.bounds;
        Vector3 rightTop = cam.WorldToViewportPoint(b.max);

        if (rightTop.x >= 0f)
        {
            outCameraTriggered = false;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(0f, jumpForce);
        anim.SetBool("isJumping", true);

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayJump();
    }

    // ================= OUT CAMERA =================
    void CheckOutOfCamera()
    {
        if (isDead || outCameraTriggered) return;

        Renderer rend = GetComponent<Renderer>();
        if (!rend) return;

        Bounds b = rend.bounds;
        Vector3 rightTop = cam.WorldToViewportPoint(b.max);

        if (rightTop.x < 0f)
        {
            Debug.Log("OUT CAMERA -> DAMAGE");

            outCameraTriggered = true;
            TakeDamage(1);
            Respawn();
        }
    }

    void Respawn()
    {
        Debug.Log("RESPAWN");

        rb.simulated = false;
        rb.velocity = Vector2.zero;

        // đẩy lên cao để tránh overlap cactus
        transform.position = respawnPosition + Vector3.up * 0.5f;

        rb.simulated = true;

        StartCoroutine(InvincibleCooldown());
    }

    // ================= COLLISION =================
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || !canTakeDamage) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint2D contact = collision.contacts[0];

            if (hitEffectPrefab)
                Instantiate(hitEffectPrefab, contact.point, Quaternion.identity);

            //Hit cactus sound
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayHit();

            TakeDamage(1);
            StartCoroutine(InvincibleCooldown());
        }

    }

    // ================= DAMAGE =================
    void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.value = currentHealth;
        
        CameraShake.Instance?.Shake();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator InvincibleCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invincibleTime);

        canTakeDamage = true;
        Debug.Log("INVINCIBLE END");
    }

    // ================= DIE =================
    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("PLAYER DIE");

        CameraShake.Instance?.Shake();
        anim.SetTrigger("Die");

        rb.velocity = Vector2.zero;
        rb.simulated = false;

        UIManager.Instance.ShowGameOver();
        Time.timeScale = 0f;
    }

    // ================= COIN =================
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (coinCollectEffectPrefab)
                Instantiate(coinCollectEffectPrefab, other.transform.position, Quaternion.identity);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCoin();

            GameManager.Instance.coins += 1;
            Destroy(other.gameObject);
        }
    }

    public void PausePlayer()
    {
        isPaused = true;
        rb.velocity = Vector2.zero;
        anim.Play("PlayerIdle", 0, 0f);
    }

    public void ResumePlayer()
    {
        if (isDead) return;
        isPaused = false;
    }
}
