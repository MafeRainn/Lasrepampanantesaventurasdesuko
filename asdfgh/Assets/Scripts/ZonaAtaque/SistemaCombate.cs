using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum EstadoCombate { Inicio, TurnoJugador, TurnoBot, Victoria, Derrota, Escapando }

public class SistemaCombate : MonoBehaviour
{
    [Header("Luchadores")]
    [SerializeField] private Luchador jugador;
    [SerializeField] private Luchador bot;

    [Header("UI")]
    [SerializeField] private Button botonAtacar;
    [SerializeField] private Button botonEscapar;
    [SerializeField] private TMP_Text textoMensaje;
    [SerializeField] private TMP_Text textoVidaJugador;
    [SerializeField] private TMP_Text textoVidaBot;

    [Header("Escenas")]
    [SerializeField] private string escenaOriginal = "AlejoDEV"; // pon aquí el nombre de tu escena original

    [Header("Tiempos")]
    [SerializeField] private float pausa = 1.2f;

    private EstadoCombate estado;

    private void Start()
    {
        StartCoroutine(IniciarCombate());
    }

    private IEnumerator IniciarCombate()
    {
        estado = EstadoCombate.Inicio;
        ActivarBotones(false);
        ActualizarUI();
        textoMensaje.text = "¡Comienza el combate!";
        yield return new WaitForSeconds(pausa);

        TurnoJugador();
    }

    private void TurnoJugador()
    {
        estado = EstadoCombate.TurnoJugador;
        textoMensaje.text = "Tu turno: elige una acción";
        ActivarBotones(true);
    }

    // Conecta al OnClick del botón "Atacar"
    public void OnBotonAtacar()
    {
        if (estado != EstadoCombate.TurnoJugador) return;
        StartCoroutine(AtaqueJugador());
    }

    // Conecta al OnClick del botón "Escapar"
    public void OnBotonEscapar()
    {
        if (estado != EstadoCombate.TurnoJugador) return;
        StartCoroutine(Escapar());
    }

    private IEnumerator AtaqueJugador()
    {
        ActivarBotones(false);

        int dano = jugador.Atacar(bot);
        ActualizarUI();
        textoMensaje.text = jugador.DescribirAtaque(bot, dano);
        yield return new WaitForSeconds(pausa);

        if (!bot.EstaVivo)
        {
            Terminar(EstadoCombate.Victoria);
            yield break;
        }

        StartCoroutine(TurnoBot());
    }

    private IEnumerator TurnoBot()
    {
        estado = EstadoCombate.TurnoBot;
        textoMensaje.text = "Turno del bot...";
        yield return new WaitForSeconds(pausa);

        int dano = bot.Atacar(jugador);
        ActualizarUI();
        textoMensaje.text = bot.DescribirAtaque(jugador, dano);
        yield return new WaitForSeconds(pausa);

        if (!jugador.EstaVivo)
        {
            Terminar(EstadoCombate.Derrota);
            yield break;
        }

        TurnoJugador();
    }

    private IEnumerator Escapar()
    {
        estado = EstadoCombate.Escapando;
        ActivarBotones(false);
        textoMensaje.text = "¡Escapaste del combate!";
        yield return new WaitForSeconds(pausa);

        SceneManager.LoadScene(escenaOriginal);
    }

    private void Terminar(EstadoCombate resultado)
    {
        estado = resultado;
        ActivarBotones(false);
        textoMensaje.text = resultado == EstadoCombate.Victoria ? "¡Ganaste!" : "Has perdido...";
    }

    private void ActivarBotones(bool activos)
    {
        botonAtacar.interactable = activos;
        botonEscapar.interactable = activos;
    }

    private void ActualizarUI()
    {
        textoVidaJugador.text = $"{jugador.nombre}: {jugador.vidaActual}/{jugador.vidaMax}";
        textoVidaBot.text = $"{bot.nombre}: {bot.vidaActual}/{bot.vidaMax}";
    }
}