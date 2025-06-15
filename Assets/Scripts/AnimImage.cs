using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimImage : MonoBehaviour
{
    [Header("Sprites de Animaci�n")]
    [Tooltip("Lista de sprites que formar�n la animaci�n, en orden secuencial.")]
    [SerializeField] private List<Sprite> frames = new List<Sprite>();

    [Header("Configuraci�n de Velocidad")]
    [Tooltip("Intervalo en segundos entre cambio de cada frame.")]
    [SerializeField] private float frameInterval = 0.1f;

    private SpriteRenderer spriteRenderer; // Referencia interna al SpriteRenderer
    private int currentFrame = 0;         // �ndice del frame actual
    private float timer = 0f;             // Temporizador interno

    void Awake()
    {
        // Obtenemos el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Si hay al menos un sprite, lo mostramos inicialmente
        if (frames.Count > 0)
        {
            spriteRenderer.sprite = frames[0];
        }
        else
        {
            Debug.LogWarning("SCR_SpriteAnimator: La lista de frames est� vac�a.");
        }
    }

    void Update()
    {
        // Solo animamos si tenemos sprites y un intervalo v�lido
        if (frames.Count == 0 || frameInterval <= 0f)
            return;

        // Avanzamos el temporizador
        timer += Time.deltaTime;

        // Cuando superamos el intervalo, cambiamos al siguiente frame
        if (timer >= frameInterval)
        {
            timer -= frameInterval;
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
        }
    }

    /// <summary>
    /// Reinicia la animaci�n al primer frame.
    /// </summary>
    public void ResetAnimation()
    {
        currentFrame = 0;
        timer = 0f;
        if (frames.Count > 0)
            spriteRenderer.sprite = frames[0];
    }
}
