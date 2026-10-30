using UnityEngine;

[System.Serializable]
public class Opcion
{
    public string texto;
    public int proximoNodoID;
}

[System.Serializable]
public class DialogoNodo
{
    public int id;
    public string nombrePersonaje;
    public string textoDialogo;
    public Opcion[] opciones;
    public int siguienteNodoID = -1;

    [Tooltip("Escena a la que se cambia al terminar este nodo. Déjalo vacío si no cambia de escena.")]
    public string escenaSiguiente;

    [Tooltip("Número de nodo con el que empieza el diálogo en la nueva escena.")]
    public int nodoEnEscenaSiguiente = 0;

    public bool TieneOpciones => opciones != null && opciones.Length > 0;
}

[System.Serializable]
public class VozPersonaje
{
    public string nombrePersonaje;
    public AudioClip sonido;
    public float pitch = 1f;
}