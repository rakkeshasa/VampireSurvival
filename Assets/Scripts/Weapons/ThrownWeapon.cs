using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    public float throwPower;
    public Rigidbody2D rb;
    public float rotateSpeed;

    void Start()
    {
        rb.linearVelocity = new Vector2(Random.Range(-throwPower, throwPower), throwPower);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * 360f * Time.deltaTime * Mathf.Sign(rb.linearVelocityX)));
    }
}
