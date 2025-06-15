using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bala : MonoBehaviour
{
    [Header("Configuración de Bala")]
    [Tooltip("Velocidad a la que la bala se mueve hacia el jugador")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Daño que inflige la bala al jugador")]
    [SerializeField] private float damage = 10f;

    [Header("Control de Combate")]
    [Tooltip("Determina si la bala debe seguir al jugador o desactivarse")]
    public bool combate = true;

    // Referencia interna al jugador
    private UnityEngine.Transform playerTransform;
    private Player playerScript;
    // Posición inicial donde nace la bala
    private Vector3 initialPosition;

    void Start()
    {
        // Guardamos la posición inicial para poder resetearla
        initialPosition = transform.position;

        // Buscamos el jugador por tag "Player"
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTransform = playerGO.transform;
            playerScript = playerGO.GetComponent<Player>();
            if (playerScript == null)
                Debug.LogError("SCR_Bullet: No se encontró el componente Player en el objeto con tag 'Player'.");
        }
        else
        {
            Debug.LogError("SCR_Bullet: No se encontró ningún GameObject con tag 'Player'. Asegúrate de que tu jugador tenga este tag.");
        }
    }

    void Update()
    {
        // Si combate se desactiva, reseteamos y apagamos la bala
        if (!combate)
        {
            ResetAndDeactivate();
            return;
        }

        // Si tenemos referencia al jugador, orientamos y movemos la bala hacia él
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            // Orientamos la bala para que su eje Y positivo apunte al jugador
            transform.up = direction;
            // Movemos la bala en esa dirección
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    // Detecta colisiones 2D (requiere Collider2D con "Is Trigger" activado)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicamos daño al jugador
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);
            }
            ResetAndDeactivate();
        }
    }

    /// <summary>
    /// Reinicia la posición de la bala a su punto inicial y la desactiva.
    /// </summary>
    private void ResetAndDeactivate()
    {
        transform.position = initialPosition;
        gameObject.SetActive(false);
    }
}
