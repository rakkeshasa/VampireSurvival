using UnityEngine;

public class WeaponThrower : Weapon
{
    public EnemyDamager damager;
    private float throwCounter;

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

        throwCounter -= Time.deltaTime;
        if(throwCounter < 0)
        {
            throwCounter = stats[weaponLevel].attackInterval;

            for(int i = 0; i < stats[weaponLevel].amount; i++)
            {
                Instantiate(damager, damager.transform.position, damager.transform.rotation).gameObject.SetActive(true);
            }

            SFXManager.instance.PitchControl(4);
        }
    }

    void SetStats()
    {
        damager.Damage = stats[weaponLevel].damage;
        damager.lifeTime = stats[weaponLevel].duration;
        damager.transform.localScale = Vector3.one * stats[weaponLevel].range;
        throwCounter = 0f;
    }
}
