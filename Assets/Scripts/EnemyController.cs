using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;
    private Transform target;
    void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = (target.position - transform.position).normalized * moveSpeed;
    }
}
