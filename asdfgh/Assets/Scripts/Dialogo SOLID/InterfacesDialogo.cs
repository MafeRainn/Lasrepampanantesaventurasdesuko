using System;

public interface IEntradaAvance
{
    bool AvancePresionado();
}

public interface ICatalogoDialogo
{
    DialogoNodo ObtenerNodo(int id);
    VozPersonaje ObtenerVoz(string nombrePersonaje);
}

public interface IVistaTexto
{
    void Mostrar(bool visible);
    void EstablecerPersonaje(string nombre);
    char[] PrepararTexto(string texto);
    void RevelarCaracteres(int cantidad);
}

public interface IVistaOpciones
{
    void MostrarOpciones(Opcion[] opciones, Action<int> alElegir);
    void OcultarOpciones();
}

public interface ISonidoLetra
{
    void Reproducir(VozPersonaje voz);
}

public interface IEscritorTexto
{
    bool Escribiendo { get; }
    void Escribir(string texto, VozPersonaje voz, Action alTerminar);
    void Completar();
    void Detener();
}