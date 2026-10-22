using System;
using UnityEngine;

public class CatalogoDialogo : MonoBehaviour, ICatalogoDialogo
{
    [SerializeField] private DialogoNodo[] nodos;
    [SerializeField] private VozPersonaje[] voces;

    public DialogoNodo ObtenerNodo(int id)
    {
        if (id < 0 || id >= nodos.Length)
        {
            Debug.LogError("El nodo " + id + " no existe en " + gameObject.name, this);
            return null;
        }

        return nodos[id];
    }

    public VozPersonaje ObtenerVoz(string nombrePersonaje)
    {
        foreach (VozPersonaje voz in voces)
        {
            if (string.Equals(voz.nombrePersonaje, nombrePersonaje, StringComparison.OrdinalIgnoreCase))
            {
                return voz;
            }
        }

        return null;
    }
}