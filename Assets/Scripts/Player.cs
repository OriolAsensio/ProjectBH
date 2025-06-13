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

    private float speedX, speedY;
    private Rigidbody2D rb;
    private Animator animator;

    private string lastDirection = "Down"; // Dirección inicial por defecto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        bool isSprinting = Input.GetButton("Fire3") && currentStamina > 0;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        speedX = Input.GetAxisRaw("Horizontal") * currentSpeed;
        speedY = Input.GetAxisRaw("Vertical") * currentSpeed;

        rb.velocity = new Vector2(speedX, speedY);

        if (isSprinting && (speedX != 0 || speedY != 0))
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }

        UpdateAnimatorParameters();
    }

    void UpdateAnimatorParameters()
    {
        Vector2 input = new Vector2(speedX, speedY);
        bool isMoving = input.sqrMagnitude > 0.01f;

        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            // Normalizamos el vector de entrada para evitar valores grandes por velocidad
            Vector2 normalizedInput = input.normalized;

            animator.SetFloat("InputX", normalizedInput.x);
            animator.SetFloat("InputY", normalizedInput.y);

            animator.SetFloat("LastInputX", normalizedInput.x);
            animator.SetFloat("LastInputY", normalizedInput.y);
        }
        else
        {
            // Si está en idle, mantenemos los últimos valores de dirección
            animator.SetFloat("InputX", 0);
            animator.SetFloat("InputY", 0);
            // No actualizamos LastInputX/LastInputY aquí para que se mantengan
        }
    }
}
