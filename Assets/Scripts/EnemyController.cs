using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float moveSpeed;
    private Transform target;

    [SerializeField]
    private float damage;

    private float delayTime = 1f;
    private float hitCounter;
    private EnemyPool enemyPool;
    private float despawnDistance = 20f;

    void Start()
    {
        // target = FindAnyObjectByType<PlayerController>().transform;
        target = PlayerHealthController.instance.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        enemyPool = FindAnyObjectByType<EnemyPool>();
    }

    void Update()
    {
        rb.linearVelocity = (target.position - transform.position).normalized * moveSpeed;
        sprite.flipX = rb.linearVelocityX > 0;
        if(hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }

        if (Vector3.Distance(target.position, transform.position) > despawnDistance)
        {
            enemyPool.ReturnEnemy(gameObject); // 몬스터 반환
            Debug.Log("Enemy Clear");
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
