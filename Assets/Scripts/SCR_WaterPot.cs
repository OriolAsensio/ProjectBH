using UnityEngine;

public class SCR_WaterPot : MonoBehaviour
{
    // Referencia al componente Player. Asignar en el Inspector o se busca automáticamente.
    [SerializeField]
    private Player player;

    // Cantidad de stamina que aporta al jugador
    [Tooltip("Cantidad de resistencia (stamina) que aportará esta jarra de agua cuando se consuma")]
    [SerializeField]
    private float staminaAmount = 10f;

    void Start()
    {
        // Si no se ha asignado en el Inspector, lo buscamos en la escena
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        // Si sigue siendo null, avisamos con un error
        if (player == null)
        {
            Debug.LogError("SCR_WaterPot: No se encontró ningún Player en la escena. Asigna la referencia en el Inspector o añade un objeto Player.");
        }
    }

    // Método que detecta colisiones 2D con "Is Trigger" activado
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos que el objeto que ha entrado tenga el componente Player
        if (other.TryGetComponent<Player>(out Player collidedPlayer))
        {
            // Garantizamos la referencia al Player correcto
            if (player == null)
                player = collidedPlayer;

            ConsumirAgua();
        }
    }

    // Lógica de sumar stamina al jugador y destruir la jarra
    private void ConsumirAgua()
    {
        if (player == null)
            return;

        // Añadimos la cantidad deseada
        player.currentStamina += staminaAmount;

        // Clampeamos para no pasarnos del máximo
        player.currentStamina = Mathf.Clamp(player.currentStamina, 0f, player.maxStamina);

        // Mensaje en consola para verificación
        Debug.Log($"Stamina aumentada en {staminaAmount}. Valor actual: {player.currentStamina}/{player.maxStamina}");

        // Destruimos la jarra para que no se pueda usar de nuevo
        Destroy(gameObject);
    }
}