using UnityEngine;

public class DialogoManager : MonoBehaviour
{
    public static bool Activo { get; private set; }

    [SerializeField] private CatalogoDialogo catalogo;
    [SerializeField] private VistaDialogoUI vista;
    [SerializeField] private EscritorTexto escritor;
    [SerializeField] private SonidoLetraAudio sonido;

    private DialogoSesion sesion;
    private IEntradaAvance entrada;
    private int frameCambio = -1;

    private void Awake()
    {
        entrada = new EntradaAvanceUnity();
        escritor.Configurar(vista, sonido);
        sesion = new DialogoSesion(catalogo, vista, vista, escritor);
        sesion.EstadoCambiado += AlCambiarEstado;
    }

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
        Activo = sesion.Activa;
        frameCambio = Time.frameCount;
    }
}