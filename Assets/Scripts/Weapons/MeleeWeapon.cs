using UnityEngine;

public class MeleeWeapon : Weapon
{
    public EnemyDamager damager;
    private float attackCounter, direction;
    void Start()
    {
        SetStats();
    }

    void Update()
    {
        if (statsUpdated)
        {
            statsUpdated = false;
            SetStats();
        }

        attackCounter -= Time.deltaTime;
        if(attackCounter < 0)
        {
            attackCounter = stats[weaponLevel].attackInterval;
            direction = Input.GetAxisRaw("Horizontal");
            if (direction != 0)
            {
                if(direction > 0)
                {
                    damager.transform.rotation = Quaternion.identity;

                }
                else
                {
                    damager.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
            }
            Instantiate(damager, damager.transform.position, damager.transform.rotation, transform).gameObject.SetActive(true);
            for(int i = 0; i < stats[weaponLevel].amount; i++)
            {
                float rot = (360f / stats[weaponLevel].amount) * i;
                Instantiate(damager, damager.transform.position, Quaternion.Euler(0f, 0f, damager.transform.rotation.eulerAngles.z + rot), transform).gameObject.SetActive(true);
            }
        }
    }

    private void SetStats()
    {
        damager.Damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        attackCounter = 0f;
    }
}
