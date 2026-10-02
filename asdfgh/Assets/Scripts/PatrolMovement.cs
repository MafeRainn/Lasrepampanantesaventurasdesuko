using UnityEngine;


public class PatrolMovement : MonoBehaviour
{

    public Transform[] PatrolPoints;
    public int PatrolDestination;
    public Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PatrolDestination == 0)

        {
            transform.position = Vector2.MoveTowards(transform.position, PatrolPoints[0].position, enemy.speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, PatrolPoints[0].position) < 0.2f)
            {
                PatrolDestination = 1;
            }

        }

        if (PatrolDestination == 1)

        {
            transform.position = Vector2.MoveTowards(transform.position, PatrolPoints[1].position, enemy.speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, PatrolPoints[1].position) < 0.2f)
            {
                PatrolDestination = 0;
            }
        }
    }
}
