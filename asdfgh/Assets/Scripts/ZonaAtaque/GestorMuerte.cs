using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Va en la escena ZonaAtaque (por ejemplo en GameManagerAtaque).
// Vigila la vida de los luchadores sin necesidad de modificar sus scripts.
public class GestorMuerte : MonoBehaviour
{
    [Header("Luchadores")]
    [SerializeField] private Luchador jugador;
    [SerializeField] private Luchador bot;

    [Header("Escenas")]
    [SerializeField] private string escenaOriginal = "SampleScene";
    [Tooltip("Si lo dejas vacío, al morir el jugador vuelve a la escena original")]
    [SerializeField] private string escenaDerrota = "";

    [Header("Identificador del bot")]
    [Tooltip("Debe ser igual al ID del script BotPersistente del bot en la escena original")]
    [SerializeField] private string idBot = "bot_1";

    [Header("Efectos")]
    [SerializeField] private float duracionFade = 1f;
    [SerializeField] private float esperaFinal = 1.5f;
    [Tooltip("Opcional: nombre del trigger del Animator para la animación de muerte")]
    [SerializeField] private string triggerMuerte = "";

    private bool procesandoMuerte;

    private void Update()
    {
        if (procesandoMuerte) return;

        if (!bot.EstaVivo)
        {
            procesandoMuerte = true;
            StartCoroutine(MuerteBot());
        }
        else if (!jugador.EstaVivo)
        {
            procesandoMuerte = true;
            StartCoroutine(MuerteJugador());
        }
    }

    private IEnumerator MuerteBot()
    {
        yield return Desvanecer(bot);

        // Se recuerda que este bot ya fue derrotado para que no reaparezca
        EstadoMundo.MarcarDerrotado(idBot);

        yield return new WaitForSeconds(esperaFinal);
        SceneManager.LoadScene(escenaOriginal);
    }

    private IEnumerator MuerteJugador()
    {
        yield return Desvanecer(jugador);
        yield return new WaitForSeconds(esperaFinal);

        string destino = string.IsNullOrEmpty(escenaDerrota) ? escenaOriginal : escenaDerrota;
        SceneManager.LoadScene(destino);
    }

    private IEnumerator Desvanecer(Luchador luchador)
    {
        Animator animator = luchador.GetComponentInChildren<Animator>();
        if (animator != null && !string.IsNullOrEmpty(triggerMuerte))
            animator.SetTrigger(triggerMuerte);

        SpriteRenderer[] sprites = luchador.GetComponentsInChildren<SpriteRenderer>();
        float t = 0f;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(t / duracionFade);

            foreach (SpriteRenderer s in sprites)
            {
                Color c = s.color;
                c.a = alpha;
                s.color = c;
            }
            yield return null;
        }
    }
}
