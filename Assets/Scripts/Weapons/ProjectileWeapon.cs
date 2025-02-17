using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public EnemyDamager damager;
    public Projectile projectile;

    private float shotCounter;
    public float weaponRange;
    public LayerMask hitEnemy;
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

        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            shotCounter = stats[weaponLevel].attackInterval;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, weaponRange * stats[weaponLevel].range, hitEnemy);
            if(enemies.Length > 0 )
            {
                for(int i = 0; i < stats[weaponLevel].amount; i++)
                {
                    // ����ü�� ���� ��ü �� �����ϰ� �̾� ������ �ֱ�
                    Vector3 targetPos = enemies[Random.Range(0, enemies.Length)].transform.position;
                    
                    Vector3 dir = targetPos - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    angle -= 90;
                    projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    Instantiate(projectile, projectile.transform.position, projectile.transform.rotation).gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetStats()
    {
        damager.Damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        shotCounter = 0f;
        projectile.moveSpeed = stats[weaponLevel].speed;
    }
}
