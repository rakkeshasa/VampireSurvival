using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    public int currentCoins;
    public CoinPickup coin;


    private void Awake()
    {
        instance = this;
    }

    public void AddCoin(int coin)
    {
        currentCoins += coin;
        UIController.instance.UpdateCoins();
    }

    public void DropCoin(Vector3 position, int value)
    {
        CoinPickup newCoin = Instantiate(coin, position + new Vector3(.2f, .1f, 0f), Quaternion.identity);
        newCoin.coinAmount = value;
        newCoin.gameObject.SetActive(true);
    }

    public void SpendCoin(int coin)
    {
        currentCoins -= coin;
        UIController.instance.UpdateCoins();
    }
}
