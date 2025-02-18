using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinAmount = 1;

    private bool magnetic;
    public float speed;

    public float timeBetweenChecks = .2f;
    private float checkCounter;

    private PlayerController player;
    void Start()
    {
        player = PlayerController.instance;
    }

    void Update()
    {
        if (magnetic)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerHealthController.instance.transform.position, speed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter < 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, PlayerHealthController.instance.transform.position) < player.pickupRange)
                {
                    magnetic = true;
                    speed += player.moveSpeed;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CoinController.instance.AddCoin(coinAmount);
            Destroy(gameObject);
        }
    }
}
