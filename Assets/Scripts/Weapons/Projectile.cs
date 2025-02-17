using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
}
