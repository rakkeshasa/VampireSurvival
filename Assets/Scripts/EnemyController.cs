using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Transform target;

    public float moveSpeed;
    public float health = 5f;

    // Damage
    [SerializeField]
    private float damage;
    private float delayTime = 1f;
    private float hitCounter;
    [SerializeField]
    private float knockBackTime = .5f;
    private float knockBackCounter;

    // EnemyPooling
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
        if(knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;

            if(moveSpeed > 0)
            {
                moveSpeed = -moveSpeed * 2f;
            }

            if(knockBackCounter <= 0)
            {
                moveSpeed = Mathf.Abs(moveSpeed * .5f);
            }
        }

        rb.linearVelocity = (target.position - transform.position).normalized * moveSpeed;

        if (knockBackCounter <= 0)
        {
            sprite.flipX = rb.linearVelocityX > 0;
        }

        if (hitCounter > 0f)
        {
            hitCounter -= Time.deltaTime;
        }

        if (Vector3.Distance(target.position, transform.position) > despawnDistance)
        {
            enemyPool.ReturnEnemy(gameObject); // 몬스터 반환
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

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            enemyPool.ReturnEnemy(gameObject);
        }

        DamageUIController.instance.SetDamageUI(damage, transform.position);
    }

    public void TakeDamage(float damage, bool shouldKnockback)
    {
        TakeDamage(damage);

        if(shouldKnockback)
        {
            knockBackCounter = knockBackTime;
        }
    }
}
