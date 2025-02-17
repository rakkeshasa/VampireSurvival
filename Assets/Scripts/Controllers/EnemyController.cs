using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Transform target;

    public float moveSpeed;
    public float health = 5f;
    public int exp = 1;

    // Damage
    [SerializeField]
    private float damage;
    [SerializeField]
    private float delayAttack = 1f;
    private float hitCounter;
    [SerializeField]
    private float knockBackTime = .5f;
    private float knockBackCounter;

    private float despawnDistance = 30f;



    void Start()
    {
        target = PlayerHealthController.instance.transform;
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if(PlayerController.instance.gameObject.activeSelf)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }

                if (knockBackCounter <= 0)
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
                Destroy(gameObject);
            }
        }
        else
        {
            // 플레이어가 죽으면 몬스터 정지
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            hitCounter = delayAttack;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
            PlayerLevelController.instance.SpawnExp(transform.position, exp);
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
