using UnityEngine;

// Ida y vuelta: 0 → 1 → 2 → 1 → 0 → 1 ...  (con 2 puntos: 0 → 1 → 0 → 1)
public class PatrolPingPong : PatrolBehaviour
{
    private int step = 1;

    protected override int GetNextIndex(int current)
    {
        if (patrolPoints.Length <= 1) return current;

        int next = current + step;
        if (next >= patrolPoints.Length || next < 0)
        {
            step = -step;
            next = current + step;
        }
        return next;
    }
}
