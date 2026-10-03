using UnityEngine;

public class Bot : Luchador
{
    [Header("Bot")]
    [Range(0f, 1f)][SerializeField] private float probCritico = 0.2f;
    [SerializeField] private float multiplicadorCritico = 1.5f;

    private bool ultimoFueCritico;

    // El bot tiene probabilidad de golpe crítico
    protected override int CalcularDano()
    {
        int dano = base.CalcularDano();
        ultimoFueCritico = Random.value < probCritico;

        if (ultimoFueCritico)
            dano = Mathf.RoundToInt(dano * multiplicadorCritico);

        return dano;
    }

    public override string DescribirAtaque(Luchador objetivo, int dano)
    {
        string critico = ultimoFueCritico ? " ¡CRÍTICO!" : "";
        return $"{nombre} te atacó e hizo {dano} de daño.{critico}";
    }
}
