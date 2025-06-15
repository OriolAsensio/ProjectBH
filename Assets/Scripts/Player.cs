using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainPerSecond = 10f;
    public Image staminaBar;

    [Header("Salud")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBar;

    [Header("Cura")]
    [Tooltip("Segundos de espera entre cada punto de vida curado")]
    public float healInterval = 0.1f;
    private float healTimer = 0f;

    private float speedX, speedY;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentStamina = maxStamina;
        currentHealth = maxHealth;

        if (staminaBar == null)
            Debug.LogWarning("Player: No se ha asignado la barra de Stamina en el Inspector.");
        if (healthBar == null)
            Debug.LogWarning("Player: No se ha asignado la barra de Salud en el Inspector.");
    }

    void Update()
    {
        // Movimiento y sprint
        bool isSprinting = Input.GetButton("Fire3") && currentStamina > 0f;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        speedX = Input.GetAxisRaw("Horizontal") * currentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * currentSpeed;
        rb.velocity = new Vector2(speedX, speedY);

        // Consumo de stamina al sprintar
        if (isSprinting && (speedX != 0f || speedY != 0f))
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // Cura al mantener presionada la tecla E
        if (Input.GetKey(KeyCode.F) && currentHealth < maxHealth && currentStamina > 0f)
        {
            healTimer += Time.deltaTime;
            while (healTimer >= healInterval)
            {
                healTimer -= healInterval;
                currentHealth += 1f;
                currentStamina -= 1f;

                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
        }
        else
        {
            // Reiniciamos el temporizador cuando no cura
            healTimer = 0f;
        }

        // Actualización de UI
        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
        if (healthBar != null)
            healthBar.fillAmount = currentHealth / maxHealth;

        UpdateAnimatorParameters();
    }

    /// <summary>
    /// Aplica daño al jugador.
    /// </summary>
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Lógica al morir el jugador.
    /// </summary>
    private void Die()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
        Debug.Log("Player ha muerto.");
    }

    void UpdateAnimatorParameters()
    {
        Vector2 input = new Vector2(speedX, speedY);
        bool isMoving = input.sqrMagnitude > 0.01f;

        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            Vector2 normalizedInput = input.normalized;
            animator.SetFloat("InputX", normalizedInput.x);
            animator.SetFloat("InputY", normalizedInput.y);
            animator.SetFloat("LastInputX", normalizedInput.x);
            animator.SetFloat("LastInputY", normalizedInput.y);
        }
        else
        {
            animator.SetFloat("InputX", 0f);
            animator.SetFloat("InputY", 0f);
        }
    }
}