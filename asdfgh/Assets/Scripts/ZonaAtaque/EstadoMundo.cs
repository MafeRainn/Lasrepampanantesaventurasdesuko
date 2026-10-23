using System.Collections.Generic;
using UnityEngine;

// Guarda información que debe sobrevivir al cambio de escena.
public static class EstadoMundo
{
    private static readonly HashSet<string> derrotados = new HashSet<string>();

    public static void MarcarDerrotado(string id)
    {
        if (!string.IsNullOrEmpty(id)) derrotados.Add(id);
    }

    public static bool EstaDerrotado(string id)
    {
        return !string.IsNullOrEmpty(id) && derrotados.Contains(id);
    }

    public static void Reiniciar()
    {
        derrotados.Clear();
    }

    // Limpia los datos cada vez que das Play en el editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarAlIniciar()
    {
        derrotados.Clear();
    }
}
