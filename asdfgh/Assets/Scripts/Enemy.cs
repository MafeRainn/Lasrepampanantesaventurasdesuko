using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    [Tooltip("Distancia (centro a centro) a la que se detiene cerca del jugador. " +
             "Debe ser un poco mayor que la suma de los radios de los colliders.")]
    public float stopDistance = 1f;

    [Header("Persecución")]
    [Tooltip("Si está activo, deja de perseguir cuando el jugador sale de la zona.")]
    public bool stopChasingOnExit = true;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform target;                      // se asigna al entrar al trigger
    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;  // hacia dónde mira al estar quieto


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moveDirection = Vector2.zero;

        if (target != null)
        {
            Vector2 toTarget = target.position - transform.position;

            if (toTarget.magnitude > stopDistance)
            {
                moveDirection = toTarget.normalized;
            }
            else
            {
                // Ya llegó: se queda quieto pero mirando al jugador
                lastDirection = SnapToCardinal(toTarget);
            }
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    private void UpdateAnimation()
    {
        bool isMoving = moveDirection != Vector2.zero;
        animator.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            lastDirection = SnapToCardinal(moveDirection);
            animator.SetFloat("InputX", lastDirection.x);
            animator.SetFloat("InputY", lastDirection.y);
        }

        // El Idle usa estos dos parámetros
        animator.SetFloat("LastInputX", lastDirection.x);
        animator.SetFloat("LastInputY", lastDirection.y);
    }

    // Convierte cualquier dirección en una de las 4 direcciones (arriba/abajo/izq/der)
    private static Vector2 SnapToCardinal(Vector2 dir)
    {
        return Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
            ? new Vector2(Mathf.Sign(dir.x), 0f)
            : new Vector2(0f, Mathf.Sign(dir.y));
    }

    // Estos eventos los dispara el CircleCollider2D (Is Trigger) del hijo "DetectionZone"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            target = other.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (stopChasingOnExit && other.CompareTag("Player"))
            target = null;
    }
}