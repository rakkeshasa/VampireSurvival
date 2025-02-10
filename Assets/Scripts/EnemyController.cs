using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;
    private Transform target;

    [SerializeField]
    private float damage;

    private float delayTime = 1f;
    private float hitCounter;

    void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = (target.position - transform.position).normalized * moveSpeed;
        
        if(hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = delayTime;
        }
    }
}
