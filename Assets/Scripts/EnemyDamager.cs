using JetBrains.Annotations;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float Damage;

    public float lifeTime, growSpeed = 1f;
    private Vector3 targetSize;

    [SerializeField] 
    private bool shouldKnockBack;

    void Start()
    {
        // Destroy(gameObject, lifeTime);

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);
        
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            targetSize = Vector3.zero;

            if(transform.localScale.x == 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyController>().TakeDamage(Damage, shouldKnockBack);
        }
    }
}
