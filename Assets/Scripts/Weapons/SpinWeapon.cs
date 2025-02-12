using UnityEngine;

public class SpinWeapon : MonoBehaviour
{
    public float rotateSpeed;
    private Transform holder;
    public Transform fireballToSpawn;

    public float timeBetweenSpawn;
    private float spawnCounter;

    void Start()
    {
        holder = transform.Find("Holder");
        // fireballToSpawn = transform.Find("FireballHolder");

    }

    void Update()
    {
        holder.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
        
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeBetweenSpawn;

            Instantiate(fireballToSpawn, fireballToSpawn.position, fireballToSpawn.rotation, holder).gameObject.SetActive(true);
        }
    }
}
