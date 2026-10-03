using UnityEngine;

// Clase abstracta: define CÓMO se patrulla hacia un punto (igual para todos)
// y deja que cada subclase decida EN QUÉ ORDEN se recorren los puntos.
// Nota: una clase abstracta no se puede añadir como componente;
// se añade una de sus subclases (PatrolPingPong, PatrolLoop, ...).
public abstract class PatrolBehaviour : MonoBehaviour
{
    [Tooltip("Puntos de la ruta. NO deben ser hijos del enemigo o se moverán con él.")]
    public Transform[] patrolPoints;
    public float arriveDistance = 0.2f;

    protected int currentIndex = 0;

    // El Enemy llama a esto cada frame y recibe la dirección en la que debe caminar
    public Vector2 GetDirection(Vector2 currentPosition)
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return Vector2.zero;

        Vector2 destination = patrolPoints[currentIndex].position;

        // Si llegó al punto, pasa al siguiente según la regla de la subclase
        if (Vector2.Distance(currentPosition, destination) < arriveDistance)
        {
            currentIndex = GetNextIndex(currentIndex);
            destination = patrolPoints[currentIndex].position;
        }

        Vector2 toDestination = destination - currentPosition;
        return toDestination.sqrMagnitude > 0.0001f ? toDestination.normalized : Vector2.zero;
    }

    // Cada subclase define el orden de la ruta
    protected abstract int GetNextIndex(int current);
}
