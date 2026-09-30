using UnityEngine;

[System.Serializable]
public class Opcion
{
    public string texto; // Lo que ve el jugador
    public int proximoNodoID; // A qué nodo va si elige esto
}

[System.Serializable]
public class DialogoNodo
{
    public int id; // ID único
    public string nombrePersonaje; // Quién habla
    public string textoDialogo; // Qué dice
    public Opcion[] opciones; // Las respuestas posibles
}