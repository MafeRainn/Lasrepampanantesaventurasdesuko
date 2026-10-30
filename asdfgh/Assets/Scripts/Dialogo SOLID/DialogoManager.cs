using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogoManager : MonoBehaviour
{
    public static bool Activo { get; private set; }

    private static int nodoAlEntrar = -1;

    [SerializeField] private CatalogoDialogo catalogo;
    [SerializeField] private VistaDialogoUI vista;
    [SerializeField] private EscritorTexto escritor;
    [SerializeField] private SonidoLetraAudio sonido;
    [SerializeField] private int nodoInicial = 0;
    [SerializeField] private UnityEvent alCerrar;

    private DialogoSesion sesion;
    private IEntradaAvance entrada;
    private int frameCambio = -1;
    private bool listo;

    private void Awake()
    {
        entrada = new EntradaAvanceUnity();
        escritor.Configurar(vista, sonido);
        sesion = new DialogoSesion(catalogo, vista, vista, escritor);
        sesion.EstadoCambiado += AlCambiarEstado;
        listo = true;
    }

    private void OnEnable()
    {
        if (!listo)
        {
            return;
        }

        int inicio = nodoAlEntrar >= 0 ? nodoAlEntrar : nodoInicial;
        nodoAlEntrar = -1;
        Iniciar(inicio);
    }

    private void OnDisable()
    {
        Activo = false;
    }

    private void Update()
    {
        if (!sesion.Activa || Time.frameCount == frameCambio || !entrada.AvancePresionado())
        {
            return;
        }

        sesion.Avanzar();
    }

    public void Iniciar(int id)
    {
        sesion.Iniciar(id);
    }

    private void AlCambiarEstado()
    {
        bool estabaActivo = Activo;
        Activo = sesion.Activa;
        frameCambio = Time.frameCount;

        if (estabaActivo && !Activo)
        {
            alCerrar.Invoke();

            DialogoNodo ultimo = sesion.UltimoNodo;
            if (ultimo != null && !string.IsNullOrEmpty(ultimo.escenaSiguiente))
            {
                nodoAlEntrar = ultimo.nodoEnEscenaSiguiente;
                SceneManager.LoadScene(ultimo.escenaSiguiente);
            }
        }
    }
}