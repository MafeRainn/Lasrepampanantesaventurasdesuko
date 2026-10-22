using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class DialogoManager : MonoBehaviour
{
    public static bool Activo { get; private set; }

    [SerializeField] private DialogoNodo[] nodos;
    [SerializeField] private VozPersonaje[] voces;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    [SerializeField] private Button[] botones;
    [SerializeField] private AudioSource fuenteAudio;
    [SerializeField] private AudioClip sonidoLetra;
    [SerializeField] private float segundosPorLetra = 0.04f;
    [SerializeField] private float variacionPitch = 0.1f;

    private Coroutine escritura;
    private DialogoNodo nodoActual;
    private VozPersonaje vozActual;
    private bool escribiendo;
    private int frameNodo;

    private void Start()
    {
        Iniciar(0);
    }

    private void OnDisable()
    {
        Activo = false;
    }

    private void Update()
    {
        if (UiManager.Pausado || nodoActual == null || Time.frameCount == frameNodo || !PresionoAvanzar())
        {
            return;
        }

        if (escribiendo)
        {
            StopCoroutine(escritura);
            textoDialogo.maxVisibleCharacters = int.MaxValue;
            FinalizarNodo();
        }
        else if (nodoActual.opciones.Length == 0)
        {
            MostrarNodo(nodoActual.siguienteNodoID);
        }
    }

    private bool PresionoAvanzar()
    {
#if ENABLE_INPUT_SYSTEM
        return (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
#endif
    }

    public void Iniciar(int id)
    {
        if (panelDialogo != null)
        {
            panelDialogo.SetActive(true);
        }

        MostrarNodo(id);
    }

    public void MostrarNodo(int id)
    {
        if (id < 0)
        {
            CerrarDialogo();
            return;
        }

        if (id >= nodos.Length)
        {
            Debug.LogError("El nodo " + id + " no existe en " + gameObject.name, this);
            return;
        }

        if (escritura != null)
        {
            StopCoroutine(escritura);
        }

        Activo = true;
        nodoActual = nodos[id];
        vozActual = BuscarVoz(nodoActual.nombrePersonaje);
        frameNodo = Time.frameCount;
        textoNombre.text = nodoActual.nombrePersonaje;
        escritura = StartCoroutine(Escribir());
    }

    private void CerrarDialogo()
    {
        nodoActual = null;
        escribiendo = false;

        if (panelDialogo != null)
        {
            panelDialogo.SetActive(false);
        }

        StartCoroutine(LiberarJugador());
    }

    private IEnumerator LiberarJugador()
    {
        yield return null;

        if (nodoActual == null)
        {
            Activo = false;
        }
    }

    private VozPersonaje BuscarVoz(string nombre)
    {
        foreach (VozPersonaje voz in voces)
        {
            if (string.Equals(voz.nombrePersonaje, nombre, System.StringComparison.OrdinalIgnoreCase))
            {
                return voz;
            }
        }

        return null;
    }

    private IEnumerator Escribir()
    {
        escribiendo = true;

        foreach (Button boton in botones)
        {
            boton.gameObject.SetActive(false);
        }

        textoDialogo.text = nodoActual.textoDialogo;
        textoDialogo.maxVisibleCharacters = 0;
        textoDialogo.ForceMeshUpdate();
        int total = textoDialogo.textInfo.characterCount;

        for (int i = 1; i <= total; i++)
        {
            textoDialogo.maxVisibleCharacters = i;
            char letra = textoDialogo.textInfo.characterInfo[i - 1].character;

            if (!char.IsWhiteSpace(letra) && !char.IsPunctuation(letra))
            {
                ReproducirLetra();
            }

            yield return new WaitForSeconds(segundosPorLetra);
        }

        FinalizarNodo();
    }

    private void FinalizarNodo()
    {
        escribiendo = false;
        ConfigurarBotones();
    }

    private void ReproducirLetra()
    {
        AudioClip clip = vozActual != null && vozActual.sonido != null ? vozActual.sonido : sonidoLetra;
        float pitchBase = vozActual != null ? vozActual.pitch : 1f;

        fuenteAudio.pitch = pitchBase + Random.Range(-variacionPitch, variacionPitch);
        fuenteAudio.PlayOneShot(clip);
    }

    private void ConfigurarBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            botones[i].onClick.RemoveAllListeners();

            if (i < nodoActual.opciones.Length)
            {
                int destino = nodoActual.opciones[i].proximoNodoID;
                botones[i].gameObject.SetActive(true);
                botones[i].GetComponentInChildren<TextMeshProUGUI>().text = nodoActual.opciones[i].texto;
                botones[i].onClick.AddListener(() => MostrarNodo(destino));
            }
            else
            {
                botones[i].gameObject.SetActive(false);
            }
        }
    }
}