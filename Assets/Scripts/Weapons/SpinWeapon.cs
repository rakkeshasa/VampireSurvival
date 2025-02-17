using UnityEngine;

public class SpinWeapon : Weapon
{
    public float rotateSpeed;
    private Transform holder;
    public Transform fireballToSpawn;

    public float spawnInterval;
    private float spawnCounter;

    public EnemyDamager damager;

    void Start()
    {
        holder = transform.Find("Holder");
        SetStats();
    }

    void Update()
    {
        // holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * stats[weaponLevel].speed));
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = spawnInterval;

            // Instantiate(fireballToSpawn, fireballToSpawn.position, fireballToSpawn.rotation, holder).gameObject.SetActive(true);
            for(int i = 0; i < stats[weaponLevel].amount; i++)
            {
                float rot = 360f / stats[weaponLevel].amount * i;

                Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);
            }
        }

        if(statsUpdated)
        {
            statsUpdated = false;
            SetStats();
        }
    }

    public void SetStats()
    {
        damager.Damage = stats[weaponLevel].damage;
        transform.localScale = Vector3.one * stats[weaponLevel].range;
        spawnInterval = stats[weaponLevel].attackInterval;
        damager.lifeTime = stats[weaponLevel].duration;
        spawnCounter = 0;
    }
}
