using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class UiManager : MonoBehaviour
{
    public static bool Pausado { get; private set; }

    [SerializeField] private bool esEscenaDeJuego;
    [SerializeField] private string escenaJuego = "Juego";
    [SerializeField] private string escenaInicio = "Pantalladeinicio";

    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelOpciones;

    [SerializeField] private Button botonEmpezar;
    [SerializeField] private Button botonSalir;
    [SerializeField] private Button botonOpciones;
    [SerializeField] private Button botonResumir;
    [SerializeField] private Button botonVolverMenu;
    [SerializeField] private Button botonCerrarOpciones;
    [SerializeField] private Slider sliderVolumen;

    private void Start()
    {
        Time.timeScale = 1f;
        Pausado = false;

        Mostrar(panelOpciones, false);
        Mostrar(panelMenu, !esEscenaDeJuego);

        Conectar(botonEmpezar, Empezar);
        Conectar(botonSalir, Salir);
        Conectar(botonOpciones, AbrirOpciones);
        Conectar(botonResumir, Reanudar);
        Conectar(botonVolverMenu, VolverAlMenu);
        Conectar(botonCerrarOpciones, CerrarOpciones);

        float volumen = PlayerPrefs.GetFloat("Volumen", 1f);
        AudioListener.volume = volumen;

        if (sliderVolumen != null)
        {
            sliderVolumen.minValue = 0f;
            sliderVolumen.maxValue = 1f;
            sliderVolumen.value = volumen;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    private void Update()
    {
        if (!PresionoEscape())
        {
            return;
        }

        if (panelOpciones != null && panelOpciones.activeSelf)
        {
            CerrarOpciones();
        }
        else if (esEscenaDeJuego)
        {
            if (Pausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    private bool PresionoEscape()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    private void Conectar(Button boton, UnityAction accion)
    {
        if (boton != null)
        {
            boton.onClick.AddListener(accion);
        }
    }

    private void Mostrar(GameObject panel, bool visible)
    {
        if (panel != null)
        {
            panel.SetActive(visible);
        }
    }

    public void Pausar()
    {
        Pausado = true;
        Time.timeScale = 0f;
        Mostrar(panelMenu, true);
    }

    public void Reanudar()
    {
        Pausado = false;
        Time.timeScale = 1f;
        Mostrar(panelMenu, false);
    }

    public void AbrirOpciones()
    {
        Mostrar(panelMenu, false);
        Mostrar(panelOpciones, true);
    }

    public void CerrarOpciones()
    {
        Mostrar(panelOpciones, false);
        Mostrar(panelMenu, true);
    }

    public void Empezar()
    {
        CargarEscena(escenaJuego);
    }

    public void VolverAlMenu()
    {
        CargarEscena(escenaInicio);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("Volumen", valor);
    }

    private void CargarEscena(string nombre)
    {
        Time.timeScale = 1f;
        Pausado = false;
        SceneManager.LoadScene(nombre);
    }
}