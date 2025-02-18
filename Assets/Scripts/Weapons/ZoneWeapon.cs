using UnityEngine;

public class ZoneWeapon : Weapon
{
    public EnemyDamager damager;

    private float spawnTime, spawnCounter;

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

        spawnCounter -= Time.deltaTime;
        if(spawnCounter < 0)
        {
            spawnCounter = spawnTime;
            Instantiate(damager, damager.transform.position, Quaternion.identity, transform).gameObject.SetActive(true);
            SFXManager.instance.PitchControl(10);
        }
    }

    void SetStats()
    {
        damager.Damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.damageInterval = stats[weaponLevel].speed;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        spawnTime = stats[weaponLevel].attackInterval;
        spawnCounter = 0f;
    }
}
