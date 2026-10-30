using System;

public class DialogoSesion
{
    private readonly ICatalogoDialogo catalogo;
    private readonly IVistaTexto vistaTexto;
    private readonly IVistaOpciones vistaOpciones;
    private readonly IEscritorTexto escritor;
    private DialogoNodo nodoActual;

    public DialogoSesion(ICatalogoDialogo catalogo, IVistaTexto vistaTexto, IVistaOpciones vistaOpciones, IEscritorTexto escritor)
    {
        this.catalogo = catalogo;
        this.vistaTexto = vistaTexto;
        this.vistaOpciones = vistaOpciones;
        this.escritor = escritor;
    }

    public bool Activa => nodoActual != null;

    public DialogoNodo UltimoNodo { get; private set; }

    public event Action EstadoCambiado;

    public void Iniciar(int id)
    {
        Mostrar(id);
    }

    public void Avanzar()
    {
        if (!Activa)
        {
            return;
        }

        if (escritor.Escribiendo)
        {
            escritor.Completar();
        }
        else if (!nodoActual.TieneOpciones)
        {
            Mostrar(nodoActual.siguienteNodoID);
        }
    }

    private void Mostrar(int id)
    {
        if (id < 0)
        {
            Cerrar();
            return;
        }

        DialogoNodo nodo = catalogo.ObtenerNodo(id);
        if (nodo == null)
        {
            return;
        }

        nodoActual = nodo;
        UltimoNodo = nodo;
        vistaTexto.Mostrar(true);
        vistaTexto.EstablecerPersonaje(nodo.nombrePersonaje);
        vistaOpciones.OcultarOpciones();
        escritor.Escribir(nodo.textoDialogo, catalogo.ObtenerVoz(nodo.nombrePersonaje), MostrarOpciones);
        EstadoCambiado?.Invoke();
    }

    private void MostrarOpciones()
    {
        if (Activa && nodoActual.TieneOpciones)
        {
            vistaOpciones.MostrarOpciones(nodoActual.opciones, Mostrar);
        }
    }

    private void Cerrar()
    {
        nodoActual = null;
        escritor.Detener();
        vistaTexto.Mostrar(false);
        EstadoCambiado?.Invoke();
    }
}