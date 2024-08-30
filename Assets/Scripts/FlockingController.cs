using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FlockingController : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 2f;
    public float neighborRadius = 1.5f;
    public float separationRadius = 0.5f;
    public float separationWeight = 1f;
    public float cohesionWeight = 1f;
    public float alignmentWeight = 1f;
    public Transform target;
    public GameObject player;
    private Rigidbody2D rb;
    private List<GameObject> neighbors;
    private Vector2 separation;
    private Vector2 cohesion;
    private Vector2 alignment;
    private float speed;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        speed = Random.Range(minSpeed, maxSpeed);
        neighbors = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    void FixedUpdate()
    {
        FindNeighbors();
        ComputeSeparation();
        ComputeCohesion();
        //ComputeAlignment();
        Move();
    }

    void FindNeighbors()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, neighborRadius);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(neighborRadius, neighborRadius), 0);

        neighbors.Clear();
        neighbors.Add(player.gameObject);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player")))
            {
                neighbors.Add(collider.gameObject);
            }
        }
    }

    void ComputeSeparation()
    {
        separation = Vector2.zero;

        foreach (GameObject neighbor in neighbors)
        {
            Vector2 difference = transform.position - neighbor.transform.position;
            float distance = difference.magnitude;

            if (distance < separationRadius)
            {
                separation += difference.normalized / distance;
            }
        }

        separation *= separationWeight;
    }

    void ComputeCohesion()
    {
        cohesion = Vector2.zero;

        foreach (GameObject neighbor in neighbors)
        {
            cohesion += (Vector2)neighbor.transform.position;
        }

        cohesion /= neighbors.Count;
        cohesion = cohesion - (Vector2)transform.position;
        cohesion *= cohesionWeight;
    }

    void ComputeAlignment()
    {
        alignment = Vector2.zero;

        foreach (GameObject neighbor in neighbors)
        {
            alignment += neighbor.GetComponent<Rigidbody2D>().velocity;
        }

        alignment /= neighbors.Count;
        alignment *= alignmentWeight;
    }
    
    void Move()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        
        Vector2 velocity = rb.velocity;
        velocity += (separation + cohesion + alignment + direction * 10).normalized * speed * Time.fixedDeltaTime;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        rb.velocity = velocity;

        //rb.MovePosition(velocity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, neighborRadius);
        //Gizmos.DrawCube(transform.position, new Vector2(neighborRadius, neighborRadius));
    }
}
