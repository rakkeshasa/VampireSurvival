using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float Damage;

    public float lifeTime, growSpeed = 1f;
    private Vector3 targetSize;

    [SerializeField] 
    private bool shouldKnockBack;
    public bool destroyParent = true;

    public bool infiniteDamager;
    public float damageInterval;
    private float damageCounter;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    public bool destroyOnImpact;

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
                if(destroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if(infiniteDamager)
        {
            damageCounter -= Time.deltaTime;

            if(damageCounter <= 0)
            {
                damageCounter = damageInterval;
                for(int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(Damage, shouldKnockBack);
                    }
                    else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!infiniteDamager)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(Damage, shouldKnockBack);
                
                if(destroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(infiniteDamager)
        {
            if(collision.tag == "Enemy")
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}
