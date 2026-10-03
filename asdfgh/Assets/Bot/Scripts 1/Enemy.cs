using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    [Tooltip("Distancia (centro a centro) a la que se detiene cerca del jugador.")]
    public float stopDistance = 1f;

    [Header("Persecución")]
    public bool stopChasingOnExit = true;

    [Header("Patrulla (opcional)")]
    [SerializeField] private PatrolBehaviour patrol;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform target;                      // se asigna al entrar al trigger
    private Vector2 moveDirection;
    private Vector2 lastDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Si no se asignó en el Inspector, busca cualquier subclase de PatrolBehaviour
        if (patrol == null)
            patrol = GetComponent<PatrolBehaviour>();
    }

    private void Update()
    {
        moveDirection = Vector2.zero;

        if (target != null)
        {
            // Prioridad 1: perseguir al jugador
            Vector2 toTarget = target.position - transform.position;

            if (toTarget.magnitude > stopDistance)
                moveDirection = toTarget.normalized;
            else
                lastDirection = SnapToCardinal(toTarget); // quieto, mirando al jugador
        }
        else if (patrol != null)
        {
            // Prioridad 2: patrullar
            moveDirection = patrol.GetDirection(transform.position);
        }

        UpdateAnimation();
    }

    // Único lugar donde se mueve el enemigo (patrulla y persecución comparten esto)
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

        animator.SetFloat("LastInputX", lastDirection.x);
        animator.SetFloat("LastInputY", lastDirection.y);
    }

    private static Vector2 SnapToCardinal(Vector2 dir)
    {
        return Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
            ? new Vector2(Mathf.Sign(dir.x), 0f)
            : new Vector2(0f, Mathf.Sign(dir.y));
    }

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
