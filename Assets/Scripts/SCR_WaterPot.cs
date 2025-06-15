using UnityEngine;

public class SCR_WaterPot : MonoBehaviour
{
    // Referencia al componente Player. Asignar en el Inspector o se busca autom�ticamente.
    [SerializeField]
    private Player player;

    // Cantidad de stamina que aporta al jugador
    [Tooltip("Cantidad de resistencia (stamina) que aportar� esta jarra de agua cuando se consuma")]
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
            Debug.LogError("SCR_WaterPot: No se encontr� ning�n Player en la escena. Asigna la referencia en el Inspector o a�ade un objeto Player.");
        }
    }

    // M�todo que detecta colisiones 2D con "Is Trigger" activado
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

    // L�gica de sumar stamina al jugador y destruir la jarra
    private void ConsumirAgua()
    {
        if (player == null)
            return;

        // A�adimos la cantidad deseada
        player.currentStamina += staminaAmount;

        // Clampeamos para no pasarnos del m�ximo
        player.currentStamina = Mathf.Clamp(player.currentStamina, 0f, player.maxStamina);

        // Mensaje en consola para verificaci�n
        Debug.Log($"Stamina aumentada en {staminaAmount}. Valor actual: {player.currentStamina}/{player.maxStamina}");

        // Destruimos la jarra para que no se pueda usar de nuevo
        Destroy(gameObject);
    }
}