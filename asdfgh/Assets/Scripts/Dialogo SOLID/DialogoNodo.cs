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
}

[System.Serializable]
public class VozPersonaje
{
    public string nombrePersonaje;
    public AudioClip sonido;
    public float pitch = 1f;
}