using UnityEngine;

// Circuito cerrado: 0 → 1 → 2 → 0 → 1 → 2 ...
public class PatrolLoop : PatrolBehaviour
{
    protected override int GetNextIndex(int current)
    {
        return (current + 1) % patrolPoints.Length;
    }
}
