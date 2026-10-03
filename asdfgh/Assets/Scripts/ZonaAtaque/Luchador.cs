using UnityEngine;

// Clase abstracta: no se puede poner directamente en un GameObject.
// Hay que usar una clase hija (Jugador, Bot, etc.).
public abstract class Luchador : MonoBehaviour
{
    [Header("Stats")]
    public string nombre = "Luchador";
    public int vidaMax = 100;
    public int vidaActual;
    public int danoMin = 8;
    public int danoMax = 15;

    public bool EstaVivo => vidaActual > 0;

    protected virtual void Awake()
    {
        vidaActual = vidaMax;
    }

    // Cada hijo puede cambiar cómo calcula su daño (críticos, buffs, etc.)
    protected virtual int CalcularDano()
    {
        return Random.Range(danoMin, danoMax + 1);
    }

    // Cada hijo DEBE definir qué mensaje muestra al atacar
    public abstract string DescribirAtaque(Luchador objetivo, int dano);

    public virtual int Atacar(Luchador objetivo)
    {
        int dano = CalcularDano();
        objetivo.RecibirDano(dano);
        return dano;
    }

    public virtual void RecibirDano(int cantidad)
    {
        vidaActual = Mathf.Max(0, vidaActual - cantidad);
    }
}