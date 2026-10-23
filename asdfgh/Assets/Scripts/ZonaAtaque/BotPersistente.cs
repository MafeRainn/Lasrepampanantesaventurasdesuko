using UnityEngine;

// Va en el bot de la ESCENA ORIGINAL (el que tiene el BotTrigger).
// Si ya fue derrotado en combate, se elimina al cargar la escena.
public class BotPersistente : MonoBehaviour
{
    [SerializeField] private string idBot = "bot_1";

    private void Awake()
    {
        if (EstadoMundo.EstaDerrotado(idBot))
        {
            Destroy(gameObject);
        }
    }
}
