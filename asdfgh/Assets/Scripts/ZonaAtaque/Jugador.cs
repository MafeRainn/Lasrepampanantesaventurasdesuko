using UnityEngine;

public class Jugador : Luchador
{
    public override string DescribirAtaque(Luchador objetivo, int dano)
    {
        return $"Atacaste a {objetivo.nombre} e hiciste {dano} de daño";
    }
}
